using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Inputs;
using CodeBase.Data;
using UnityEngine;
using CodeBase.Logic;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private const string LAYER_NAME = "Hittable";

        [SerializeField]
        private HeroAnimator _animator;

        [SerializeField]
        private CharacterController _characterController;

        private IInputService _input;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private HeroStats _stats;

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
                _hits[i].transform.parent
                    .GetComponent<IHealth>()
                    .TakeDamage(_stats.Damage);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer(LAYER_NAME);
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !_animator.IsAttacking)
                _animator.PlayAttack();
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(AttackPoint(), _stats.AttackRadius, _hits, _layerMask);

        private Vector3 AttackPoint() =>
            StartPoing() + transform.forward;

        private Vector3 StartPoing() =>
            new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);

    }
}