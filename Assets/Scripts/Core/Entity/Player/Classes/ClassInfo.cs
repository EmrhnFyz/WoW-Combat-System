using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Class Info", menuName = "Game Data/Classes/Class Info", order = 1)]
    public class ClassInfo : ScriptableUniqueInfo<ClassInfo>
    {
        [SerializeField] private ClassInfoContainer container;
        [SerializeField] private bool isAvailable;
        [SerializeField] private ClassType classType;
        [SerializeField] private List<SpellPowerTypeInfo> powerTypes;
        [SerializeField] private List<SpellInfo> classSpells;

        private readonly HashSet<SpellInfo> spellInfoHashSet = new();
        private readonly HashSet<SpellPowerType> spellPowerTypeInfoHashSet = new();

        protected override ScriptableUniqueInfoContainer<ClassInfo> Container => container;
        protected override ClassInfo Data => this;

        public bool IsAvailable => isAvailable;
        public ClassType ClassType => classType;
        public SpellPowerType MainPowerType => powerTypes[0].PowerType;
        public IReadOnlyList<SpellInfo> ClassSpells => classSpells;
        public IReadOnlyList<SpellPowerTypeInfo> PowerTypes => powerTypes;

        protected override void OnRegister()
        {
            base.OnRegister();

            spellInfoHashSet.IntersectWith(ClassSpells);
            foreach (SpellPowerTypeInfo powerTypeInfo in powerTypes)
            {
                spellPowerTypeInfoHashSet.Add(powerTypeInfo.PowerType);
            }
        }

        protected override void OnUnregister()
        {
            spellInfoHashSet.Clear();
            spellPowerTypeInfoHashSet.Clear();

            base.OnUnregister();
        }

        public bool HasSpell(SpellInfo spellInfo) => spellInfoHashSet.Contains(spellInfo);
        public bool HasPower(SpellPowerType spellPowerType) => spellPowerTypeInfoHashSet.Contains(spellPowerType);
    }
}
