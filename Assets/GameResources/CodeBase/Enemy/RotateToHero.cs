using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        [SerializeField]
        private float _speed = 1f;

        private Transform _heroTransform;

        private Vector3 _positionToLook;

        public void Constructor(Transform heroTransform) =>
            _heroTransform = heroTransform;

        private void Update()
        {
            if (Initialized())
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

        private Quaternion TargetRotation(Vector3 position) => 
            Quaternion.LookRotation(position);

        private float SpeedFactor() => 
            _speed * Time.deltaTime;

        private bool Initialized() => _heroTransform != null;
    }
}
