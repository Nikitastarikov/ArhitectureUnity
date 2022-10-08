using UnityEngine;

namespace CodeBase.Services.Inputs
{
    public abstract class InputService : IInputService
    {
        protected const string HORIZONTAL_STR = "Horizontal";
        protected const string VERTICAL_STR = "Vertical";
        private const string FIRE_STR = "Fire";
        public abstract Vector2 Axis { get; }
        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(FIRE_STR);

        protected static Vector2 SimpleInputAxis() => 
            new Vector2(SimpleInput.GetAxis(HORIZONTAL_STR), SimpleInput.GetAxis(VERTICAL_STR));
    }
}