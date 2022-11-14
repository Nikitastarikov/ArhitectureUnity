using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using System.Collections.Generic;
using CodeBase.StaticData;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgress> ProgressWriters { get; }
        public List<ISavedProgressReader> ProgressReaders { get; }

        public void Cleanup();
        public GameObject CreateHud();
        public GameObject CreateHero(GameObject at);
        public void CreateEnemySpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        public GameObject CreateMonsters(MonsterTypeId monsterTypeId, Transform parent);
        public LootPiece CreateLoot();
        void Unregister(ISavedProgressReader progressReader);
    }
}