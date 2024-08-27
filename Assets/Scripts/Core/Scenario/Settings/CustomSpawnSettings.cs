using System;
using UnityEngine;

namespace Core.Scenario
{
    [Serializable]
    public class CustomSpawnSettings
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private UnitInfoAI unitInfoAI;
        [SerializeField] private string customNameId;
        [SerializeField] private float customScale = 1.0f;

        public Transform SpawnPoint => spawnPoint;
        public UnitInfoAI UnitInfoAI => unitInfoAI;
        public string CustomNameId => customNameId;
        public float CustomScale => customScale;
    }
}
