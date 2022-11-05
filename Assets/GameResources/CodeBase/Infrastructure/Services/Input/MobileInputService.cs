using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

namespace CodeBase.Services.Inputs
{
    /// <summary>
    /// Service mobile for input.
    /// </summary>
    public class MobileInputService : InputService
    {
        /// <summary>
        /// Axes of movement of the player.
        /// </summary>
        public override Vector2 Axis => SimpleInputAxis();
    }
}