﻿using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.AssetManagement;
using System.Collections.Generic;
using CodeBase.StaticData;
using CodeBase.Logic;
using CodeBase.Enemy;
using UnityEngine.AI;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject hero;
        private readonly IStaticDataService _staticData;
        private readonly IAssets _assets;

        public GameFactory(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        public GameObject CreateHud() =>
            InstantiateRegistered(AssetPath.HUD_PATH);

        public GameObject CreateHero(GameObject at)
        {
            hero = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position);
            return hero;
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
            Attack attack = monster.GetComponent<Attack>();

            attack.Constructor(hero.transform);
            attack.Damage = monsterData.Damage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;
            attack.Cleavage = monsterData.Cleavage;

            monster.GetComponent<RotateToHero>()?.Constructor(hero.transform);

            return monster;
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
