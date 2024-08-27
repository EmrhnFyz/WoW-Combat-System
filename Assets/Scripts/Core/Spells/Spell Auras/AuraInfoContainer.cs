using Common;
using Core.AuraEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Aura Info Container", menuName = "Game Data/Containers/Aura Info", order = 1)]
    internal class AuraInfoContainer : ScriptableUniqueInfoContainer<AuraInfo>
    {
        [SerializeField] private List<AuraInfo> auraInfos;

        private readonly HashSet<int> stealthAuras = new();

        protected override List<AuraInfo> Items => auraInfos;

        public override void Register()
        {
            base.Register();

            foreach (AuraInfo auraInfo in auraInfos)
            {
                for (var i = 0; i < auraInfo.AuraEffects.Count; i++)
                {
                    if (auraInfo.AuraEffects[i] is AuraEffectInfoStealth)
                    {
                        stealthAuras.Add(auraInfo.Id);
                    }
                }
            }
        }

        public override void Unregister()
        {
            stealthAuras.Clear();

            base.Unregister();
        }

        public bool IsStealthAura(int auraId) => stealthAuras.Contains(auraId);

    }
}
