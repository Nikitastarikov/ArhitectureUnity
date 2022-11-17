using UnityEngine;
using TMPro;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField]
        private TextMeshProUGUI SkullText;

        protected override void Initialize() =>
          RefreshSkullText();

        protected override void SubscribeUpdates() =>
          _progress.WorldData.LootData.Changed += SubscribeUpdates;

        protected override void CleanUp()
        {
            base.CleanUp();
            _progress.WorldData.LootData.Changed -= SubscribeUpdates;
        }

        private string RefreshSkullText() =>
          SkullText.text = _progress.WorldData.LootData.Collected.ToString();
    }
}
