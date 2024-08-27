using Client.UI;
using UnityEngine;

namespace Client
{
    public class BattleScreen : UIWindowController<BattleScreen>
    {
        [SerializeField] private TransformBattleTagDictionary tagsByKeys;
        [SerializeField] private BattleHudPanel battleHudPanel;

        public new void Initialize(ScreenController controller)
        {
            base.Initialize(controller);

            gameObject.SetActive(false);
            tagsByKeys.Register();

            RegisterPanel<BattleHudPanel, BattleHudPanel.RegisterToken>(battleHudPanel);
        }

        public new void Deinitialize(ScreenController controller)
        {
            UnregisterPanel<BattleHudPanel, BattleHudPanel.UnregisterToken>(battleHudPanel);

            tagsByKeys.Unregister();
            gameObject.SetActive(false);

            base.Deinitialize(controller);
        }

        public RectTransform FindTag(BattleHudTagType tagType) => tagsByKeys.Value(tagType);
    }
}
