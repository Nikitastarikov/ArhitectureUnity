using CodeBase.Infrastructure.Services;
using CodeBase.UI.Services.Windows;
using CodeBase.StaticData.Windows;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.StaticData
{
    public interface IStaticDataService : IService
    {
        LevelStaticData ForLevel(string sceneKey);
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        WindowConfig ForWindow(WindowId shop);
        void LoadMosters();
    }
}