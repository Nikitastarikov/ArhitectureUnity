using CodeBase.Data;
using UnityEngine;
using TMPro;
using CodeBase.Infrastructure.PersistentProgress;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System;

namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour, ISavedProgress
    {
        [SerializeField]
        private TextMeshProUGUI _counter;
        private WorldData _worldData = default;

        public void Constructor(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.Changed += UpdateCounter;
        }
        public void LoadProgress(PlayerProgress progress) =>
            _counter.text = $"{progress.WorldData.LootData.Collected}";

        public void UpdateProgress(PlayerProgress progress) =>
            progress.WorldData.LootData.Collected = Int32.Parse(_counter.text);

        private void Start() => 
            UpdateCounter();

        private void UpdateCounter() => 
            _counter.text = $"{_worldData.LootData.Collected}";
    }

}
