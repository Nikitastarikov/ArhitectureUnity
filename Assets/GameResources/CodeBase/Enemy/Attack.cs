using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField]
        private EnemyAnimator _animator = default;

        private IGameFactory _factory = default;
        private Transform _heroTransform = default;

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.onHeroCreated += OnHeroCreated;
        }

        private void OnHeroCreated() => 
            _heroTransform = _factory.HeroGameObject.transform;
    }
}
