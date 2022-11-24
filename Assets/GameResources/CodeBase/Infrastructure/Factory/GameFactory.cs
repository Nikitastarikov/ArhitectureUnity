using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic.EnemySpawners;
using System.Collections.Generic;
using CodeBase.UI.Elements;
using CodeBase.StaticData;
using CodeBase.Services;
using CodeBase.Logic;
using CodeBase.Enemy;
using UnityEngine.AI;
using UnityEngine;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject hero;
        private readonly IWindowService _windowService;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAssets _assets;

        public GameFactory(IAssets assets, IStaticDataService staticData, 
            IPersistentProgressService progressService, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _windowService = windowService;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HUD_PATH);

            LootCounter lootCounter = hud.GetComponentInChildren<LootCounter>();
            lootCounter.Constructor(_progressService.Progress.WorldData);
            
            Register(lootCounter);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Constructor(_windowService);
            }

            return hud;
        }

        public GameObject CreateHero(Vector3 at)
        {
            hero = InstantiateRegistered(AssetPath.HERO_PATH, at);
            return hero;
        }

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.LOOT_PATH).GetComponent<LootPiece>();

            lootPiece.Constructor(_progressService.Progress.WorldData, this);

            return lootPiece;
        }

        public GameObject CreateMonsters(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = UnityEngine.Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Constructor(health);
            monster.GetComponent<AgentMoveToHero>().Constructor(hero.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
            lootSpawner.Constructor(this);

            Attack attack = monster.GetComponent<Attack>();
            attack.Constructor(hero.transform);
            attack.Damage = monsterData.Damage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;
            attack.Cleavage = monsterData.Cleavage;

            monster.GetComponent<RotateToHero>()?.Constructor(hero.transform);

            return monster;
        }

        public void CreateEnemySpawner  (Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            EnemySpawnPoint spawner = InstantiateRegistered(AssetPath.ENEMY_SPAWNER, at)
                .GetComponent<EnemySpawnPoint>();

            spawner.Constructor(this);
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }

            ProgressReaders.Add(progressReader);
        }

        public void Unregister(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Remove(progressWriter);
            }

            ProgressReaders.Remove(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }
    }
}
