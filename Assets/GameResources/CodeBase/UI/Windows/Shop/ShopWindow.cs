using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Services;
using UnityEngine;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField]
        private TextMeshProUGUI SkullText;

        [SerializeField]
        private RewardedAdItem _adItem;

        public void Constructor(IAdsService adsService, IPersistentProgressService progressService)
        {
            base.Constructor(progressService);
            _adItem.Constructor(adsService, progressService);
        }

        protected override void Initialize()
        {
            _adItem.Initialize();
            RefreshSkullText();
        }

        protected override void SubscribeUpdates()
        {
            _adItem.Subscribe();
            _progress.WorldData.LootData.Changed += SubscribeUpdates;
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _adItem.CleanUp();
            _progress.WorldData.LootData.Changed -= SubscribeUpdates;
        }

        private string RefreshSkullText() =>
          SkullText.text = _progress.WorldData.LootData.Collected.ToString();
    }
}
