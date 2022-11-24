using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        [SerializeField]
        private string _transferTo;
        private IGameStateMachine _stateMachine;

        private bool _triggered = false;

        private void Awake() => 
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            _stateMachine.Enter<LoadLevelState, string>(_transferTo);
            _triggered = true;
        }
    }
}
