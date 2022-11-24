﻿using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Infrastructure.Factory;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using CodeBase.UI.Elements;
using CodeBase.CameraLogic;
using CodeBase.StaticData;
using CodeBase.Services;
using CodeBase.Enemy;
using CodeBase.Logic;
using CodeBase.Data;
using UnityEngine;
using System;
using CodeBase.UI.Services.Factory;

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
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() => _curtain.Hide();

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() => 
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            InitSpawners(levelData);
            InitLoot();

            GameObject hero = InitHero(levelData);

            InitHud(hero);
            CameraFollow(hero);
        }

        private GameObject InitHero(LevelStaticData levelData) =>
            _gameFactory.CreateHero(levelData.InitialHeroPosition);

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

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
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
