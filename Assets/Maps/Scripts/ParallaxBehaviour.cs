using UnityEngine;

namespace MapSystem
{
    public class ParallaxBehaviour : MonoBehaviour
    {
        // Members
        [Header("Settings")]
        [SerializeField] private float _ParallaxEffectX;
        [SerializeField] private float _ParallaxEffectY;
        
        // Private
        private Transform _CameraTransform;
        private Vector3 _LastCameraPosition;

        private void Awake ()
        {
            _CameraTransform = Camera.main.transform;
            _LastCameraPosition = _CameraTransform.position;
        }

        private void FixedUpdate ()
        {
            Vector3 deltaMovement = _CameraTransform.position - _LastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * _ParallaxEffectX, deltaMovement.y * _ParallaxEffectY, 0f);
            _LastCameraPosition = _CameraTransform.position;
        }
    }
}