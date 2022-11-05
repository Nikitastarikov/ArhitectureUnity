using UnityEngine;
using System;
using System.Collections;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
    public class EnemyDeath : MonoBehaviour
    {
        public event Action onDeathHappened = delegate { };

        private const float DESTROY_TIME = 3f;

        [SerializeField]
        private EnemyAnimator _animator;

        [SerializeField]
        private EnemyHealth _health;

        [SerializeField]
        private GameObject _deathFX;

        [SerializeField]
        private AgentMoveToHero _move;

        private void Start() => 
            _health.onHealthChanged += HealthChanged;

        private void OnDestroy() => 
            _health.onHealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (_health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _health.onHealthChanged -= HealthChanged;

            _move.enabled = false;

            _animator.PlayDeath();

            SpawnDeathFX();

            StartCoroutine(DestroyTimer());

            onDeathHappened();
        }

        private void SpawnDeathFX() => 
            Instantiate(_deathFX, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DESTROY_TIME);

            Destroy(gameObject);
        }
    }
}
