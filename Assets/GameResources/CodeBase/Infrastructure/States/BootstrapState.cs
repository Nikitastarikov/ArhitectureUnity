using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Inputs;
using System;
using UnityEngine;

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

        private void EnterLoadLevel() => _gameStateMachine.Enter<LoadLevelState, string>("Main");

        private void RegisterServices()
        {
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(AllServices.Container.Single<IAssets>()));
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
