using UnityEngine.Advertisements;
using UnityEngine;
using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
    {
        public event Action onLoadedAds;
        public bool IsAdsLoaded => isAdsLoaded;
        public int Reward => reward;

        private const string ANDROID_GAME_ID = "Rewarded_Android";
        private const string IOS_GAME_ID = "Rewarded_iOS";

        private Action _onAdsFinished;

        private string _gameId = string.Empty;

        private bool isAdsLoaded = false;
        private int reward = 10;

        public void Initialize()
        {

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = ANDROID_GAME_ID;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOS_GAME_ID;
                    break;
                default:
                    Debug.LogError("Unsupported platform for ads");
                    break;
            }

            if (!_gameId.Equals(string.Empty))
            {
                Advertisement.Initialize(_gameId, true, this);
                Advertisement.Load(_gameId, this);
            }
        }

        public void ShowReWardedAds(Action onAdsFinished)   
        {
            Advertisement.Show(_gameId, this);
            _onAdsFinished = onAdsFinished;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            isAdsLoaded = placementId.Equals(_gameId);
            onLoadedAds?.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.LogError($"Error loading Ad Unit {placementId}: {error.ToString()} - {message}");

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(_gameId))
            {
                switch (showCompletionState)
                {
                    case UnityAdsShowCompletionState.COMPLETED:
                        _onAdsFinished?.Invoke();
                        break;
                    default:
                        Debug.Log($"Ads Rewarded Ad {showCompletionState}");
                        break;
                }

                _onAdsFinished = null;
                // Load another ad:
                Advertisement.Load(_gameId, this);
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}");
        public void OnUnityAdsShowStart(string placementId) { }
        public void OnUnityAdsShowClick(string placementId) { }

        public void OnInitializationComplete() => 
            Debug.Log("OnInitializationComplete");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => 
            Debug.Log($"Error showing Ad Unit: {error} - {message}");
    }
}
 