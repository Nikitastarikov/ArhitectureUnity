using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactoy : IService
    {
        void CreateShop();
        void CreateUIRoot();
    }
}