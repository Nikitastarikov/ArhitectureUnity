using UnityEngine.Advertisements;
using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action onLoadedAds;
        bool IsAdsLoaded { get; }
        int Reward { get; }

        void Initialize();
        void ShowReWardedAds(Action onAdsFinished);
    }
}