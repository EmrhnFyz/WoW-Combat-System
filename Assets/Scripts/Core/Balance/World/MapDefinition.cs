using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Map Definition", menuName = "Game Data/World/Map Definition", order = 2)]
    public class MapDefinition : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private bool isAvailable;
        [SerializeField] private string mapName;
        [SerializeField] private int maxPlayers = 10;
        [SerializeField] private float maxVisibilityRange = 10.0f;
        [SerializeField] private MapType mapType;
        [SerializeField] private Expansion expansion;
        [SerializeField] private Sprite slotBackground;

        public int Id => id;
        public bool IsAvailable => isAvailable;
        public string MapName => mapName;
        public int MaxPlayers => maxPlayers;
        public float MaxVisibilityRange => maxVisibilityRange;
        public MapType MapType => mapType;
        public Expansion Expansion => expansion;
        public Sprite SlotBackground => slotBackground;

        public bool IsDungeon()
        {
            return MapType is MapType.Instance or MapType.Raid;
        }
    }
}