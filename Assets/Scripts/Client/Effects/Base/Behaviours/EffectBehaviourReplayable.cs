using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class EffectBehaviourReplayable : EffectBehaviour
    {
        [SerializeField] private List<ParticleSystem> replayableSystems;

        protected override void OnPlay()
        {
            base.OnPlay();

            replayableSystems.ForEach(system => system.Play());
        }

        protected override void OnReplay()
        {
            OnPlay();
        }
    }
}
