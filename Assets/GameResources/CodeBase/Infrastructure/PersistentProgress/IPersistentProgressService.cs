using CodeBase.Infrastructure.Services;
using CodeBase.Data;

namespace CodeBase.Infrastructure.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}
