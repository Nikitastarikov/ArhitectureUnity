using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField]
        private Image _imageCurrent;

        public void SetValue(float current, float max) => 
            _imageCurrent.fillAmount = current / max;
    }
}
