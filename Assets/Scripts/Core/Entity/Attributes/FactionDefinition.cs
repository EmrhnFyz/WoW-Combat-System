﻿using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Faction Definition", menuName = "Game Data/Gameplay/Faction Definition", order = 1)]
    public class FactionDefinition : ScriptableObject
    {
        [SerializeField] private string localizationId;
        [SerializeField] private int factionId;
        [SerializeField] private Team team;
        [SerializeField] private List<FactionDefinition> hostileFactions;
        [SerializeField] private List<FactionDefinition> friendlyFactions;

        public HashSet<FactionDefinition> HostileFactions { get; } = new HashSet<FactionDefinition>();
        public HashSet<FactionDefinition> FriendlyFactions { get; } = new HashSet<FactionDefinition>();
        public string LocalizationId => localizationId;
        public int FactionId => factionId;
        public Team Team => team;

        private void OnEnable()
        {
            HostileFactions.Clear();
            FriendlyFactions.Clear();

            foreach (FactionDefinition hostileFaction in hostileFactions)
            {
                HostileFactions.Add(hostileFaction);
            }

            foreach (FactionDefinition friendlyFaction in friendlyFactions)
            {
                FriendlyFactions.Add(friendlyFaction);
            }
        }

        private void OnValidate()
        {
            OnEnable();
        }
    }
}
