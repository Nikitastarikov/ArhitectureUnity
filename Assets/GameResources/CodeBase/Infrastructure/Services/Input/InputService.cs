using UnityEngine;
using UnityEngine.Animations;

namespace CodeBase.Services.Inputs
{
    public class InputService : IInputService
    {
        public virtual Vector2 Axis => _input.Player.Move.ReadValue<Vector2>();

        protected PlayerInput _input;

        public InputService()
        {
            _input = new PlayerInput();

            _input.Enable();
        }

        public virtual bool IsAttackButtonUp() => _input.Player.Attack.IsPressed();

        ~InputService() => _input.Disable();
    }
}