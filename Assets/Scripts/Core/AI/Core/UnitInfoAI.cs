using Common;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Unit AI Info", menuName = "Game Data/AI/AI Info", order = 1)]
    public class UnitInfoAI : ScriptableUniqueInfo<UnitInfoAI>
    {
        [SerializeField] private UnitInfoAIContainer container;
        [SerializeField] private UnitInfoAISettings settings;

        public new int Id => base.Id;

        protected override ScriptableUniqueInfoContainer<UnitInfoAI> Container => container;
        protected override UnitInfoAI Data => this;

        public IUnitAIModel CreateAI() => settings.CreateAI();
    }
}
