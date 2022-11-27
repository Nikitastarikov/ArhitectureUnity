using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.StaticData;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgress> ProgressWriters { get; }
        List<ISavedProgressReader> ProgressReaders { get; }

        void Cleanup();
        Task<GameObject> CreateHud();
        Task<GameObject> CreateHero(Vector3 at);
        Task CreateEnemySpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        Task<GameObject> CreateMonsters(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        void Unregister(ISavedProgressReader progressReader);
        Task WarmUp();
    }
}