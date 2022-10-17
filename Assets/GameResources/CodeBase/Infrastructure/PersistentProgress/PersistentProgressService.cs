using CodeBase.Data;

namespace CodeBase.Infrastructure.PersistentProgress
{

    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}
