using CodeBase.Services;
using UnityEngine.UI;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        protected IPersistentProgressService _progressService;
        protected PlayerProgress _progress => _progressService.Progress;

        [SerializeField]
        private Button _closeButton;

        public void Constructor(IPersistentProgressService progressService) => 
            _progressService = progressService;

        private void Awake() => 
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            CleanUp();

        protected virtual void OnAwake() => 
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        protected virtual void Initialize() { }
        protected virtual void SubscribeUpdates() { }
        protected virtual void CleanUp() { }
    }
}
