using Common;
using UnityEngine;

namespace Core
{
    public class SpellCastBehaviour : UnitStateMachineBehaviour
    {
        [SerializeField] private SpellInfo spellInfo;
        [SerializeField] private int castIntervalMin;
        [SerializeField] private int castIntervalMax;

        private TimeTracker castTimeTracker = new();

        protected override void OnStart()
        {
            base.OnStart();

            castTimeTracker.Reset(RandomUtils.Next(castIntervalMin, castIntervalMax));
        }

        protected override void OnActiveUpdate(int deltaTime)
        {
            base.OnActiveUpdate(deltaTime);

            castTimeTracker.Update(deltaTime);
            if (castTimeTracker.Passed)
            {
                Unit.Spells.CastSpell(spellInfo, new SpellCastingOptions());
                castTimeTracker.Reset(RandomUtils.Next(castIntervalMin, castIntervalMax));
            }
        }
    }
}
