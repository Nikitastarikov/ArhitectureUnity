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
            RegisterStaticData();

            _services.RegisterSingle(InputService());
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IUIFactoy>(new UIFactory(_services.Single<IAssets>(), 
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>()));
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

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMosters();
            _services.RegisterSingle(staticData);
        }

        public void Exit()
        {
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }
            else
            {
                return new MobileInputService();
            }
        }
    }
}
