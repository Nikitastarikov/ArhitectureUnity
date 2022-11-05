using CodeBase.Logic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public event Action onHealthChanged = delegate { };

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        [SerializeField]
        private EnemyAnimator _animator;

        [SerializeField]
        private float _current;
        [SerializeField]
        private float _max;

        public void TakeDamage(float damage)
        {
            _current -= damage;
            _animator.PlayHit();
            onHealthChanged();
        }
    }
}
