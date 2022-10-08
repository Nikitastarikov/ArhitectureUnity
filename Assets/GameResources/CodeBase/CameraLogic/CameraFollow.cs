using System;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    /// <summary>
    ///  онтроллирует движени€ камеры, в зависимости от передвижени€ игрока.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private float rotationAngleX;
        [SerializeField]
        private int distance;
        [SerializeField]
        private float offsetY;

        [SerializeField]
        private Transform _following;

        /// <summary>
        /// ”станавливает трансформ, за которым будет следовать камера.
        /// </summary>
        /// <param name="following"></param>
        public void SetFollow(Transform following) => _following = following;

        private void LateUpdate()
        {
            if (_following == null)
                return;
            Quaternion rotation = Quaternion.Euler(rotationAngleX, 0, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + FollowingPointPosition();
            transform.rotation = rotation;
            transform.position = position;
        }
        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += offsetY;
            return followingPosition;
        }
    }
}