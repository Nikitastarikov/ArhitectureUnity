using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero : Follow
    {
        private const float MIN_DISTANCE = 1f;

        [SerializeField]
        private NavMeshAgent _agent;

        private Transform _heroTransform;
        private IGameFactory _factory;

        private void Start()
        {
            _factory = AllServices.Container.Single<IGameFactory>();

            if (HeroExists())
                InitializeHeroTransform();
            else
                _factory.onHeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            if (Initialized() && HeroNotReached())
                _agent.destination = _heroTransform.position;
        }

        private bool HeroExists() =>
            _factory.HeroGameObject != null;

        private bool Initialized() => _heroTransform != null;

        private void InitializeHeroTransform() => 
            _heroTransform = _factory.HeroGameObject.transform;

        private bool HeroNotReached() =>
            Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MIN_DISTANCE;
    }
}
