using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Inputs;
using UnityEngine;

namespace CodeBase.Hero
{
    /// <summary>
    /// Класс отвечающий за движение игрока.
    /// </summary>
    public class HeroMove : MonoBehaviour
    {
        private CharacterController characterController;

        [SerializeField]
        private float movementSpeed = 4.0f;

        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();

            characterController = GetComponent<CharacterController>();
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
            
            characterController.Move(movementSpeed * movementVector * Time.deltaTime);
        }
    }
}