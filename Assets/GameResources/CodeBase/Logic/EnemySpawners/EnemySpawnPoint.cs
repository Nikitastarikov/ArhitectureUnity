using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using CodeBase.Enemy;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;

        public string Id { get; set; }

        [SerializeField]
        private bool _slain;

        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public void Constructor(IGameFactory factory)
        {
            _factory = factory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (!progress.KillData.ClearedSpawners.Contains(Id))
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private void OnDestroy()
        {
            if (_enemyDeath != null)
                _enemyDeath.onDeathHappened -= Slay;
        }

        private async void Spawn()
        {
            GameObject monster = await _factory.CreateMonsters(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.onDeathHappened += Slay;
        }

        private void Slay() => _slain = true;
    }
}
