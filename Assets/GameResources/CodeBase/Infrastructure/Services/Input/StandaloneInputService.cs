using UnityEngine;

namespace CodeBase.Services.Inputs
{
    /// <summary>
    /// Input service through standalone.
    /// </summary>
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = SimpleInputAxis();
                if (axis == Vector2.zero)
                {
                    axis = UnityAxis();
                }
                return axis;
            }
        }

        private static Vector2 UnityAxis() => 
            new Vector2(UnityEngine.Input.GetAxis(HORIZONTAL_STR), UnityEngine.Input.GetAxis(VERTICAL_STR));
    }
}