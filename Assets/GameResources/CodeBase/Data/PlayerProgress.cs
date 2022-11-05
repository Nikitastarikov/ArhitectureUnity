using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public HeroState HeroState;
        public WorldData WorldData;
        public HeroStats HeroStats;
        public KillData KillData;

        public PlayerProgress(string initialScene)
        {
            WorldData = new WorldData(initialScene);
            HeroState = new HeroState();
            HeroStats = new HeroStats();
            KillData = new KillData();
        }

    }
}
