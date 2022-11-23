using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Services;
using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        [SerializeField]
        private Button _showAdButton;

        [SerializeField]
        private GameObject[] _adActiveObjects;
        [SerializeField]
        private GameObject[] _adInactiveObjects;
        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Constructor(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _showAdButton.onClick.AddListener(OnShowAdClick);
            RefrashAvailableAd();
        }

        public void Subscribe() => 
            _adsService.onLoadedAds += RefrashAvailableAd;

        public void CleanUp() => 
            _adsService.onLoadedAds -= RefrashAvailableAd;

        private void OnShowAdClick() => 
            _adsService.ShowReWardedAds(OnReWardedAdsComplete);

        private void OnReWardedAdsComplete() => 
            _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

        private void RefrashAvailableAd()
        {
            bool isAdsLoaded = _adsService.IsAdsLoaded;

            foreach (var adActiveObject in _adActiveObjects)
                adActiveObject.SetActive(isAdsLoaded);

            foreach (var adInactiveObject in _adInactiveObjects)
                adInactiveObject.SetActive(!isAdsLoaded);
        }
    }
}
