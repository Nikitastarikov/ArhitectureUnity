using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        public event Action<Collider> onTriggerEnter = delegate { };
        public event Action<Collider> onTriggerExit = delegate { };

        private void OnTriggerEnter(Collider other) => 
            onTriggerEnter(other);

        private void OnTriggerExit(Collider other) => 
            onTriggerExit(other);
    }
}
