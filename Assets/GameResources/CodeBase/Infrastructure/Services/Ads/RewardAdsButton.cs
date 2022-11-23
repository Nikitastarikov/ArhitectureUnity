using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class RewardAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public Button _rewardButton;

        private const string ANDROID_GAME_ID = "Rewarded_Android";
        private const string IOS_GAME_ID = "Rewarded_iOS";

        private string _gameId = string.Empty;

        private void Awake()
        {
#if UNITY_EDITOR
            _gameId = IOS_GAME_ID;
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = ANDROID_GAME_ID;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOS_GAME_ID;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = ANDROID_GAME_ID;
                    break;
                default:
                    Debug.LogError("Unsupported platform for ads");
                    break;
            }
#endif
            
        }

        private void Start()
        {
            LoadAd();   
        }

        private void LoadAd()
        {
            Debug.Log("Loaded Ad");
            Advertisement.Load(_gameId, this);
        }

        public void ShowAd()
        {
            _rewardButton.interactable = false;

            Advertisement.Show(_gameId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("OnUnityAdsAdLoaded");

            if (placementId.Equals(_gameId))
            {
                _rewardButton.onClick.AddListener(ShowAd);
                _rewardButton.interactable = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.LogError($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log("OnUnityAdsShowComplete");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        public void OnUnityAdsShowStart(string placementId) { }
        public void OnUnityAdsShowClick(string placementId) { }
    }
}
 