using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private Transform _transform;

        private void Start()
        {
            _camera = Camera.main;
            _transform = transform;
        }

        private void FixedUpdate()
        {
            Quaternion rotation = _camera.transform.rotation;
            _transform.LookAt(_transform.position + rotation * Vector3.back, rotation * Vector3.up);
        }
    }
}
