using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class DoRandomEmoteBehaviour : UnitStateMachineBehaviour
    {
        [SerializeField] private List<EmoteType> randomEmotes;
        [SerializeField] private int emoteIntervalMin;
        [SerializeField] private int emoteIntervalMax;

        private TimeTracker emoteTimeTracker = new();

        protected override void OnStart()
        {
            base.OnStart();

            emoteTimeTracker.Reset(RandomUtils.Next(emoteIntervalMin, emoteIntervalMax));
        }

        protected override void OnActiveUpdate(int deltaTime)
        {
            base.OnActiveUpdate(deltaTime);

            emoteTimeTracker.Update(deltaTime);
            if (emoteTimeTracker.Passed && randomEmotes.Count > 0)
            {
                Unit.ModifyEmoteState(RandomUtils.GetRandomElement(randomEmotes));
                emoteTimeTracker.Reset(RandomUtils.Next(emoteIntervalMin, emoteIntervalMax));
            }
        }
    }
}
