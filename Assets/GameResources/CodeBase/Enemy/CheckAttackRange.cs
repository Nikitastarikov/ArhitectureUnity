using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        
        public Attack _attack;

        [SerializeField]
        private TriggerObserver _triggerObserver;

        private void Start()
        {
            _triggerObserver.onTriggerEnter += TriggerEnter;
            _triggerObserver.onTriggerExit += TriggerExit;

            _attack.DisableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            _attack.DisableAttack();
        }

        private void TriggerEnter(Collider obj)
        {
            _attack.EnableAttack();
        }
    }
}
