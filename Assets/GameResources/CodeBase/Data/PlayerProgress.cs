using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public HeroState HeroState;
        public WorldData WorldData;
        public HeroStats HeroStats;
        public KillData KillData;
        public KnockedOutLoot KnokedOutLoot;

        public PlayerProgress(string initialScene)
        {
            WorldData = new WorldData(initialScene);
            HeroState = new HeroState();
            HeroStats = new HeroStats();
            KillData = new KillData();
            KnokedOutLoot = new KnockedOutLoot();
        }

    }

    [Serializable]
    public class KnockedOutLoot
    {
        public List<LootOnLevel> NotCollectedLoot = new List<LootOnLevel>();
    }

    [Serializable]
    public class LootOnLevel
    {
        public string Id;
        public Loot Loot;
        public PositionOnLevel PositionOnLevel;

        public LootOnLevel(string id, PositionOnLevel positionOnLevel, Loot loot)
        {
            Id = id;
            PositionOnLevel = positionOnLevel;
            Loot = loot;
        }
    }
}
