using Core.Scenario;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MapSettings : MonoBehaviour
    {
        [Serializable]
        private class ArenaSpawnInfo
        {
            [SerializeField] private Team team;
            [SerializeField] private List<Transform> spawnPoints;

            public Team Team => team;
            public List<Transform> SpawnPoints => spawnPoints;
        }

        [SerializeField, Range(2.0f, 50.0f)] private float gridCellSize;
        [SerializeField] private Transform defaultSpawnPoint;
        [SerializeField] private BoxCollider boundingBox;
        [SerializeField] private BalanceReference balance;
        [SerializeField] private MapDefinition mapDefinition;
        [SerializeField] private List<ArenaSpawnInfo> spawnInfos;
        [SerializeField] private List<ScenarioAction> scenarioActions;

        internal float GridCellSize => gridCellSize;
        internal BoxCollider BoundingBox => boundingBox;
        internal Transform DefaultSpawnPoint => defaultSpawnPoint;
        internal BalanceReference Balance => balance;
        internal MapDefinition Definition => mapDefinition;

        internal List<ScenarioAction> ScenarioActions => scenarioActions;

        public List<Transform> FindSpawnPoints(Team team)
        {
            return spawnInfos.Find(spawnInfo => spawnInfo.Team == team).SpawnPoints;
        }

#if UNITY_EDITOR
        [ContextMenu("Collect scenario actions")]
        private void CollectScenario()
        {
            scenarioActions = new List<ScenarioAction>(GetComponentsInChildren<ScenarioAction>());
        }
#endif
    }
}