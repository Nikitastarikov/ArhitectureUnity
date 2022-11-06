using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using System;
using System.Collections.Generic;
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
        public void Register(ISavedProgressReader progressReader);
        public GameObject CreateMonsters(MonsterTypeId monsterTypeId, Transform parent);
    }
}