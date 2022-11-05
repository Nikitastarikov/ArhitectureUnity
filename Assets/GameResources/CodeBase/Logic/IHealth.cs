using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {

        event Action onHealthChanged;
        /// <summary>
        /// Health max.
        /// </summary>
        float Max { get; set; }

        /// <summary>
        /// Current health.
        /// </summary>
        float Current { get; set; }

        void TakeDamage(float damage);
    }

}