using CodeBase.Infrastructure.PersistentProgress;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Inputs;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    /// <summary>
    /// Класс отвечающий за движение игрока.
    /// </summary>
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        private CharacterController _characterController;

        [SerializeField]
        private float movementSpeed = 4.0f;

        private IInputService _inputService;
        private Camera _camera;

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        public void UpdateProgress(PlayerProgress progress) => 
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (CurrentLevel().Equals(progress.WorldData.PositionOnLevel.Level) 
                && savedPosition != null)
            {
                Warp(to: savedPosition);
            }
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Start() => _camera = Camera.main;

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                //Трансформируем экранныые координаты вектора в мировые
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            
            _characterController.Move(movementSpeed * movementVector * Time.deltaTime);
        }
    }
}