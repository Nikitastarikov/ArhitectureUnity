using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Factory;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using CodeBase.CameraLogic;
using CodeBase.Services;
using CodeBase.Enemy;
using CodeBase.Logic;
using CodeBase.Data;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    internal partial class LoadLevelState : IPayloadedState<string>
    {
        private const string INITIAL_POINT_TAG = "InitialPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public LoadLevelState(GameStateMachine gameStateMachine, 
            SceneLoader sceneLoader, 
            LoadingCurtain curtain, 
            IGameFactory gameFactory, 
            IPersistentProgressService progressService, 
            IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
        }
        public void Enter(string payload)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() => _curtain.Hide();

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld()
        {
            InitSpawners();
            InitLoot();

            GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(INITIAL_POINT_TAG));
            
            InitHud(hero);
            CameraFollow(hero);
        }

        private void InitLoot()
        {
            for (int i = 0; i < GetNotCollectedLoot().Count; i++)
            {
                if (CurrentLevel().Equals(GetNotCollectedLoot()[i].PositionOnLevel.Level))
                {
                    LootPiece loot = _gameFactory.CreateLoot();
                }
            }
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;

        private List<LootOnLevel> GetNotCollectedLoot() =>
            _progressService.Progress.KnokedOutLoot.NotCollectedLoot;

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            StaticData.LevelStaticData levelData = _staticDataService.ForLevel(sceneKey);

            foreach (var spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateEnemySpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();

            hud.GetComponent<ActorUI>().
                Constructor(hero.GetComponent<IHealth>());
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main
            .GetComponent<CameraFollow>()
            .SetFollow(hero.transform);
        }
    }
}
