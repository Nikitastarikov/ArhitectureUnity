using CodeBase.Infrastructure.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string PATH_TO_STATIC_DATA_MONSTERS = "StaticData/Monsters";
        private const string PATH_TO_STATIC_DATA_SPAWNERS = "StaticData/levels";
        private const string PATH_TO_STATIC_DATA_WINDOWS = "StaticData/UI/WindowStaticData";
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void LoadMosters()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>(PATH_TO_STATIC_DATA_MONSTERS)
                .ToDictionary(x => x.MonsterTypeId, x => x);

            _levels = Resources.LoadAll<LevelStaticData>(PATH_TO_STATIC_DATA_SPAWNERS)
                .ToDictionary(x => x.LevelKey, x => x);

            _windowConfigs = Resources.Load<WindowStaticData>(PATH_TO_STATIC_DATA_WINDOWS)
                .Configs.ToDictionary(x => x.WindowId, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) => 
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig) ? windowConfig : null;
    }
}
