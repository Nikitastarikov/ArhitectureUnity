using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        private const string LAYER_PLAYER_NAME = "Player";

        [SerializeField]
        private EnemyAnimator _animator = default;

        [SerializeField]
        private float _attackCooldown = 3f;
        [SerializeField]
        private float _cleavage = 0.5f;
        [SerializeField]
        private float _effectiveDistance = 0.5f;
        [SerializeField]
        private float damage = 10f;

        private IGameFactory _factory = default;
        private Transform _heroTransform = default;
        private float _currentAttackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;

        public void DisableAttack() => _attackIsActive = false;

        public void EnableAttack() => _attackIsActive = true;

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.onHeroCreated += OnHeroCreated;

            _layerMask = 1 << LayerMask.NameToLayer(LAYER_PLAYER_NAME);
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnDestroy() =>
            _factory.onHeroCreated -= OnHeroCreated;

        private void OnAttackEnded()
        {
            _currentAttackCooldown = _attackCooldown;
            _isAttacking = false;
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), _cleavage, 1f);
                hit.transform.GetComponent<IHealth>().TakeDamage(damage);
            }
        }

        private bool CanAttack() =>
            _attackIsActive && !_isAttacking && CooldownIsUp();

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _isAttacking = true;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), _cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
            transform.forward * _effectiveDistance;

        private bool CooldownIsUp() => _currentAttackCooldown <= 0f;

        private void OnHeroCreated() =>
            _heroTransform = _factory.HeroGameObject.transform;
    }
}
