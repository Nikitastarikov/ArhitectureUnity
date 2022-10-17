using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Factory;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PROGRESS_KEY = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public PlayerProgress LoadProgress() => PlayerPrefs.GetString(PROGRESS_KEY)?.ToDeserialized<PlayerProgress>();

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.UpdateProgress(_progressService.Progress);
            }

            PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());
        }
    }
}
