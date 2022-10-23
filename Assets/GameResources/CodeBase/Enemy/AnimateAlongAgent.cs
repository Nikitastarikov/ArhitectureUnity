using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MIN_VELOCITY = 0.1f;

        [SerializeField]
        private NavMeshAgent _agent;
        [SerializeField]
        private EnemyAnimator _animator;

        private void Update()
        {
            if (ShouldMove())
                _animator.Move(_agent.velocity.magnitude);
            else
                _animator.StopMove();
        }

        private bool ShouldMove() =>
            _agent.velocity.magnitude > MIN_VELOCITY && _agent.remainingDistance > _agent.radius;
    }
}
