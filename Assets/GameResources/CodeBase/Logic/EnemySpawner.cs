using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Data;
using UnityEngine;
using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Factory;
using CodeBase.Enemy;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(UniqueId))]
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        [SerializeField]
        private MonsterTypeId MonsterTypeId;

        [SerializeField]
        private bool _slain;

        private string _id;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
            {
                _slain = true;
            }
            else
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var monster = _factory.CreateMonsters(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.onDeathHappened += Slay;
        }

        private void Slay() => _slain = true;

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        private void OnDestroy()
        {
            if (_enemyDeath != null)
                _enemyDeath.onDeathHappened -= Slay;
        }
    }
}
