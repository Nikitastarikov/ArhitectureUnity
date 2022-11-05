using UnityEngine;
using CodeBase.Hero;
using CodeBase.Logic;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField]
        private HpBar _hpBar;

        private IHealth _health;

        public void Constructor(IHealth health)
        {
            _health = health;

            _health.onHealthChanged += UpdateHpBar;
        }

        private void Start()
        {
            if (_health == null)
            {
                _health = GetComponent<IHealth>();

                if (_health != null)
                    Constructor(_health);
            }
        }

        private void OnDestroy() => 
            _health.onHealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            _hpBar.SetValue(_health.Current, _health.Max);
    }
}
