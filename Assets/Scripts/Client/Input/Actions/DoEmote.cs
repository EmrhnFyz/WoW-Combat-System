using Core;
using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Do Emote", menuName = "Player Data/Input/Actions/Do Emote", order = 2)]
    public class DoEmote : InputAction
    {
        [SerializeField] private InputReference input;
        [SerializeField] private EmoteType emoteType;

        public override void Execute() => input.DoEmote(emoteType);
    }
}
