using CodeBase.Infrastructure.Services;
using CodeBase.Logic;

namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        public MonsterStaticData ForMonster(MonsterTypeId typeId);
        public void LoadMosters();
    }
}