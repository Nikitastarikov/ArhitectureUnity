using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactoy
    {
        private const string UI_ROOT_PATH = "UI/UIRoot";
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        
        private Transform _uiRoot;
        private IPersistentProgressService _progressService;

        public UIFactory(IAssets assetProvider, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assets = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateUIRoot() => 
            _uiRoot = _assets.Instantiate(UI_ROOT_PATH).transform;

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            WindowBase windowBase = Object.Instantiate(config.Prefab, _uiRoot);
            windowBase.Constructor(_progressService);
        }
    }
}