using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Data;
using UnityEngine;
using System;
using CodeBase.Logic;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        public event Action onHealthChanged = delegate { };

        [SerializeField]
        private HeroAnimator _animator;
        private HeroState _state;

        public float Max
        {
            get => _state.MaxHp;
            set => _state.MaxHp = value;
        }
        public float Current
        {
            get => _state.CurrentHp;
            set
            {
                if (_state.CurrentHp != value)
                {
                    _state.CurrentHp = value;
                    onHealthChanged();
                }
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.MaxHp = Max;
            progress.HeroState.CurrentHp = Current;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            onHealthChanged();
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            _animator.PlayHit();
        }
    }
}