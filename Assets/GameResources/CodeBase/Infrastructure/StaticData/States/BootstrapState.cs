using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Inputs;
using CodeBase.StaticData;
using CodeBase.Services;
using UnityEngine;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using CodeBase.Infrastructure.Services.Ads;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string START_SCENE_NAME = "Initial";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _gameStateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(START_SCENE_NAME, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() => _gameStateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            RegisterStaticDataService();
            RegisterAdsService();

            _services.RegisterSingle<IGameStateMachine>(_gameStateMachine);
            _services.RegisterSingle(InputService());
            RegisterAssetProvider();
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IUIFactoy>(new UIFactory(_services.Single<IAssets>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IAdsService>()));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactoy>()));
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssets>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
        }

        private void RegisterAssetProvider()
        {
            AssetProvider assetProvider = new AssetProvider();
            assetProvider.Initialize();
            _services.RegisterSingle<IAssets>(assetProvider);
        }

        private void RegisterAdsService()
        {
            var adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterSingle<IAdsService>(adsService);
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMosters();
            _services.RegisterSingle(staticData);
        }

        public void Exit()
        {
        }

        private IInputService InputService() => new InputService();
    }
}
