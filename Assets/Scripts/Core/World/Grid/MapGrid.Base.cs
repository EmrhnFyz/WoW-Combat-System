using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal sealed partial class MapGrid
    {
        private const int GridRelocatorTime = 500;
        private const int GridRelocatorOutOfRangeTimer = 2000;

        private readonly List<WorldEntity> relocatableEntities = new();
        private readonly HashSet<WorldEntity> visibilityChangedEntities = new();
        private readonly PlayerVisibilityNotifier playerVisibilityNotifier;
        private readonly CreatureVisibilityNotifier creatureVisibilityNotifier;
        private readonly CellRelocator gridCellRelocator;
        private readonly Cell invalidCell;
        private readonly Cell[,] cells;
        private readonly Map map;
        private readonly int cellCountX;
        private readonly int cellCountZ;
        private readonly float gridCellSize;

        private TimeTracker gridCellRelocatorTimer = new(GridRelocatorTime);
        private TimeTracker gridCellOutOfRangeTimer = new(GridRelocatorOutOfRangeTimer);

        internal MapGrid(Map map)
        {
            this.map = map;

            gridCellRelocator = new CellRelocator(this);
            playerVisibilityNotifier = new PlayerVisibilityNotifier(this);
            creatureVisibilityNotifier = new CreatureVisibilityNotifier(this);
            gridCellSize = map.Settings.GridCellSize;

            cellCountX = Mathf.CeilToInt(map.Settings.BoundingBox.bounds.size.x / gridCellSize);
            cellCountZ = Mathf.CeilToInt(map.Settings.BoundingBox.bounds.size.z / gridCellSize);

            cells = new Cell[cellCountX, cellCountZ];
            Vector3 minBounds = map.Settings.BoundingBox.bounds.min;
            var origin = new Vector3(minBounds.x, map.Settings.BoundingBox.center.y, minBounds.z);
            var cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            for (var i = 0; i < cellCountX; i++)
            {
                var xOffset = (i + 0.5f) * gridCellSize;
                for (var j = 0; j < cellCountZ; j++)
                {
                    var zOffset = (j + 0.5f) * gridCellSize;
                    Vector3 cellCenter = origin + new Vector3(xOffset, 0.0f, zOffset);
                    cells[i, j] = new Cell(i, j, new Bounds(cellCenter, cellSize));
                }
            }

            invalidCell = new Cell(-1, -1, new Bounds(Vector3.zero, Vector3.positiveInfinity));
        }

        internal void Dispose()
        {
            invalidCell.Dispose();

            for (var i = 0; i < cellCountX; i++)
            {
                for (var j = 0; j < cellCountZ; j++)
                {
                    cells[i, j].Dispose();
                }
            }
        }

        internal void DoUpdate(int deltaTime)
        {
            gridCellRelocatorTimer.Update(deltaTime);
            gridCellOutOfRangeTimer.Update(deltaTime);

            if (gridCellRelocatorTimer.Passed)
            {
                for (var i = 0; i < cellCountX; i++)
                {
                    for (var j = 0; j < cellCountZ; j++)
                    {
                        cells[i, j].Visit(gridCellRelocator);
                    }
                }

                foreach (WorldEntity worldEntity in visibilityChangedEntities)
                {
                    worldEntity.IsVisibilityChanged = false;
                }

                foreach (WorldEntity relocatableEntity in relocatableEntities)
                {
                    Cell currentCell = relocatableEntity.CurrentCell;
                    Cell nextCell = FindCell(relocatableEntity.Position);
                    if (nextCell == null)
                    {
                        relocatableEntity.Position = map.Settings.DefaultSpawnPoint.position;
                        nextCell = FindCell(relocatableEntity.Position);
                    }

                    if (currentCell != nextCell)
                    {
                        currentCell.RemoveWorldEntity(relocatableEntity);
                        nextCell.AddWorldEntity(relocatableEntity);
                    }
                }

                visibilityChangedEntities.Clear();
                relocatableEntities.Clear();

                gridCellRelocatorTimer.Reset(GridRelocatorTime);
                if (gridCellOutOfRangeTimer.Passed)
                {
                    gridCellOutOfRangeTimer.Reset(GridRelocatorOutOfRangeTimer);
                }
            }
        }

        internal void AddEntity(WorldEntity entity)
        {
            Cell startingCell = FindCell(entity.Position);

            if (startingCell == null)
            {
                entity.Position = map.Settings.DefaultSpawnPoint.position;
                startingCell = FindCell(entity.Position);
            }

            Assert.IsNotNull(startingCell, $"Starting cell is not found for {entity.GetType()} at {entity.Position}");
            startingCell.AddWorldEntity(entity);
        }

        internal void RemoveEntity(WorldEntity entity)
        {
            Assert.IsNotNull(entity.CurrentCell, $"Cell is missing on removal for {entity.GetType()} at {entity.Position}");
            entity.CurrentCell.RemoveWorldEntity(entity);
        }

        internal void UpdateVisibility(Player player, bool forceUpdateOthers)
        {
            playerVisibilityNotifier.Configure(player, forceUpdateOthers);
            VisitInRadius(player, map.Settings.Definition.MaxVisibilityRange, playerVisibilityNotifier);
            playerVisibilityNotifier.Complete();
        }

        internal void UpdateVisibility(Creature creature)
        {
            creatureVisibilityNotifier.Configure(creature);
            VisitInRadius(creature, map.Settings.Definition.MaxVisibilityRange, creatureVisibilityNotifier);
            creatureVisibilityNotifier.Complete();
        }

        internal void VisitInRadius(WorldEntity referer, float radius, IUnitVisitor unitVisitor)
        {
            var cellRange = Mathf.CeilToInt(radius / gridCellSize);
            Cell originCell = referer.CurrentCell;
            if (originCell == null)
            {
                return;
            }

            var minX = Mathf.Clamp(originCell.X - cellRange, 0, cellCountX - 1);
            var maxX = Mathf.Clamp(originCell.X + cellRange, 0, cellCountX - 1);
            var minZ = Mathf.Clamp(originCell.Z - cellRange, 0, cellCountZ - 1);
            var maxZ = Mathf.Clamp(originCell.Z + cellRange, 0, cellCountZ - 1);

            for (var i = minX; i <= maxX; i++)
            {
                for (var j = minZ; j <= maxZ; j++)
                {
                    Drawing.DrawLine(referer.Position + Vector3.up, cells[i, j].Center + (Vector3.up * 20), Color.red, 1.0f);
                    Drawing.DrawLine(cells[i, j].MaxBounds + (Vector3.up * 20), cells[i, j].MinBounds + (Vector3.up * 20), cells[i, j] != originCell ? Color.green : Color.yellow, 1.0f);

                    cells[i, j].Visit(unitVisitor);
                }
            }
        }

        private Cell FindCell(Vector3 position)
        {
            Vector3 offset = position - cells[0, 0].MinBounds;
            var xCell = Mathf.FloorToInt(offset.x / map.Settings.GridCellSize);
            var zCell = Mathf.FloorToInt(offset.z / map.Settings.GridCellSize);

            if (xCell < 0 || xCell > cellCountX - 1 || zCell < 0 || zCell > cellCountZ - 1)
            {
                return null;
            }

            return cells[xCell, zCell];
        }
    }
}