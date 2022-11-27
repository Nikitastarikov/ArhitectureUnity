using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Services.Factory;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.UI.Elements;
using CodeBase.CameraLogic;
using CodeBase.StaticData;
using CodeBase.Services;
using CodeBase.Enemy;
using CodeBase.Logic;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    internal partial class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactoy _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain curtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticDataService,
            IUIFactoy uiFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }
        public void Enter(string payload)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() => _curtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitLoot();
            GameObject hero = await InitHero(levelData);
            await InitHud(hero);
            CameraFollow(hero);
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData) =>
            await _gameFactory.CreateHero(levelData.InitialHeroPosition);

        private async Task InitLoot()
        {
            for (int i = 0; i < GetNotCollectedLoot().Count; i++)
            {
                if (CurrentLevel().Equals(GetNotCollectedLoot()[i].PositionOnLevel.Level))
                {
                    LootPiece loot = await _gameFactory.CreateLoot();
                }
            }
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateEnemySpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();

            hud.GetComponent<ActorUI>().
                Constructor(hero.GetComponent<IHealth>());
        }

        private LevelStaticData LevelStaticData() => 
            _staticDataService.ForLevel(SceneManager.GetActiveScene().name);

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;

        private List<LootOnLevel> GetNotCollectedLoot() =>
            _progressService.Progress.KnokedOutLoot.NotCollectedLoot;

        private void CameraFollow(GameObject hero)
        {
            Camera.main
            .GetComponent<CameraFollow>()
            .SetFollow(hero.transform);
        }
    }
}
