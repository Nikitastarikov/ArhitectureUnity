using CodeBase.Infrastructure.Services;
using CodeBase.StaticData;

namespace CodeBase.Services
{
    public interface IStaticDataService : IService
    {
        public LevelStaticData ForLevel(string sceneKey);
        public MonsterStaticData ForMonster(MonsterTypeId typeId);
        public void LoadMosters();
    }
}