using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected = 0;
        public Action Changed = delegate { };

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed();
        }
    }
}
