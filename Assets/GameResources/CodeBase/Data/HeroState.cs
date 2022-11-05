using System;

namespace CodeBase.Data
{
    [Serializable]
    public class HeroState
    {
        public float CurrentHp;
        public float MaxHp;

        public void ResetHp() => CurrentHp = MaxHp;
    }
}
