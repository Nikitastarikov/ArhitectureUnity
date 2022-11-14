using CodeBase.Infrastructure.Services;
using CodeBase.Data;

namespace CodeBase.Services
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}
