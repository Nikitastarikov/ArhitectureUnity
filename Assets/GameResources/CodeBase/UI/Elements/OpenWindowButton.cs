using CodeBase.UI.Services.Windows;
using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField]
        private Button _openButton;

        [SerializeField]
        private WindowId _windowId;

        private IWindowService _windowService;

        public void Constructor(IWindowService windowService) => 
            _windowService = windowService;

        private void Awake() => 
            _openButton.onClick.AddListener(Open);

        private void Open() => 
            _windowService.Open(_windowId);
    }
}
