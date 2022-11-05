using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField]
        private HeroHealth _heroHealth;

        [SerializeField]
        private HeroMove _move;

        [SerializeField]
        private HeroAttack _attack;

        [SerializeField]
        private HeroAnimator _animator;

        [SerializeField]
        private GameObject _deathFX;

        private bool _isDied = false;

        private void Start() =>
            _heroHealth.onHealthChanged += HealthChanged;

        private void OnDestroy() =>
            _heroHealth.onHealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_isDied && _heroHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            _move.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(_deathFX, transform.position, Quaternion.identity);
        }
    }
}