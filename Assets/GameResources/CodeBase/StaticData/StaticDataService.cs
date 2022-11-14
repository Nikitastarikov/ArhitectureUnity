using System.Collections.Generic;
using CodeBase.Services;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string PATH_TO_STATIC_DATA_MONSTERS = "StaticData/Monsters";
        private const string PATH_TO_STATIC_DATA_SPAWNERS = "StaticData/levels";
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;

        public void LoadMosters()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>(PATH_TO_STATIC_DATA_MONSTERS)
                .ToDictionary(x => x.MonsterTypeId, x => x);

            _levels = Resources.LoadAll<LevelStaticData>(PATH_TO_STATIC_DATA_SPAWNERS)
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) => 
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;
    }
}
