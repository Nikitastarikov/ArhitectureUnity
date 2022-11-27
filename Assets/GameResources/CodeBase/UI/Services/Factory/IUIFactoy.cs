using CodeBase.Infrastructure.Services;
using System.Threading.Tasks;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactoy : IService
    {
        void CreateShop();
        Task CreateUIRoot();
    }
}