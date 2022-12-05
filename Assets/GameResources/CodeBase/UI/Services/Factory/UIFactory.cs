using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactoy
    {
        private const string UI_ROOT = "UIRoot";
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        
        private Transform _uiRoot;
        private IPersistentProgressService _progressService;
        private IAdsService _adsService;

        public UIFactory(
            IAssets assetProvider, 
            IStaticDataService staticData, 
            IPersistentProgressService progressService, 
            IAdsService adsService)
        {
            _assets = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assets.Instantiate(UI_ROOT);
            _uiRoot = prefab.transform;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow shopWindow = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            shopWindow.Constructor(_adsService, _progressService);
        }
    }
}