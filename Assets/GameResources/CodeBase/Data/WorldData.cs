using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LootData LootData;

        public WorldData(string initialScene)
        {
            PositionOnLevel = new PositionOnLevel(initialScene);
            LootData = new LootData();
        }
    }
}
