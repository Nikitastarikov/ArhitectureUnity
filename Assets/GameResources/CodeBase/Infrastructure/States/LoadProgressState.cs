using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Data;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMacine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMacine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMacine = gameStateMacine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMacine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() => 
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialScene: "Main");

            progress.HeroState.MaxHp = 50;
            progress.HeroStats.Damage = 1f;
            progress.HeroStats.AttackRadius = 0.5f;

            progress.HeroState.ResetHp();

            return progress;
        }
    }
}
