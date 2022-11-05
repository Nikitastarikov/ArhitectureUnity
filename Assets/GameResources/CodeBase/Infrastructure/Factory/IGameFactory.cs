using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public event Action onHeroCreated;

        public GameObject HeroGameObject { get; }

        public List<ISavedProgress> ProgressWriters { get; }
        public List<ISavedProgressReader> ProgressReaders { get; }

        public void Cleanup();
        public void CreateHud();
        public GameObject CreateHero(GameObject at);
    }
}