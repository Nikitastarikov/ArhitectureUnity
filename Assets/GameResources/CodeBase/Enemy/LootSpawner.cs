using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private int _lootMax;
        private int _lootMin;

        public void Constructor(IGameFactory factory) => 
            _factory = factory;

        public void SetLoot(int min, int max)
        {
            _lootMax = max;
            _lootMin = min;
        }

        private void Start() => 
            EnemyDeath.onDeathHappened += SpawnLoot;

        private async void SpawnLoot()
        {
            LootPiece loot = await _factory.CreateLoot();
            loot.transform.position = transform.position;
            Loot lootItem = GenerateLoot();

            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot()
            {
                Value = Random.Range(_lootMin, _lootMax)
            };
        }
    }
}
