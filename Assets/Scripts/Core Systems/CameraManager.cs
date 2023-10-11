using UnityEngine;
using UnityEngine.InputSystem;

using CharacterSystem;

namespace CoreSystems
{
    public class CameraManager : MonoBehaviour
    {
        // Members
        public static CameraManager Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Camera _Camera; 
        [SerializeField] private bool _Draw;

        private Transform _CameraTransform, _FollowCharacter;
        private Bounds _MapBounds, _CameraBounds;
        private Vector3 _StartPosition, _Difference;
        private bool _IsActive, _IsDrag;
        private Vector3 _Velocity;

#region Unity Functions
        private void Awake ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);

            _CameraTransform = _Camera.transform;
            _CameraBounds = new Bounds();
            _FollowCharacter = null;
            _IsActive = false;

            //GameSettings.KeyInput
        }

        private void Update ()
        {
            if (!_IsActive || _FollowCharacter != null) return;

            if (Mathf.Abs(Input.mouseScrollDelta.y) < 0.1) return;

            _Camera.orthographicSize -= Input.mouseScrollDelta.y / 5;
            _Camera.orthographicSize = Mathf.Clamp(_Camera.orthographicSize, GameSettings.CameraMinSize, GameSettings.CameraMaxSize);
            UpdateBoundary();
            ClampCamera();
        }

        private void FixedUpdate ()
        {
            if (!_IsActive) return;

            if (_FollowCharacter == null) DragCameraUpdate();
            else FollowCameraUpdate();
        }

        private void OnDrawGizmos ()
        {
            if (!_Draw || _CameraBounds == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(_CameraBounds.min.x, _CameraBounds.min.y, 0f), new Vector3(_CameraBounds.max.x, _CameraBounds.min.y, 0f));
            Gizmos.DrawLine(new Vector3(_CameraBounds.max.x, _CameraBounds.min.y, 0f), new Vector3(_CameraBounds.max.x, _CameraBounds.max.y, 0f));
            Gizmos.DrawLine(new Vector3(_CameraBounds.max.x, _CameraBounds.max.y, 0f), new Vector3(_CameraBounds.min.x, _CameraBounds.max.y, 0f));
            Gizmos.DrawLine(new Vector3(_CameraBounds.min.x, _CameraBounds.max.y, 0f), new Vector3(_CameraBounds.min.x, _CameraBounds.min.y, 0f));
        }
#endregion
#region Public Functions
        public void Activate () => _IsActive = true;
        public void Deactivate () => _IsActive = false;

        public void SetMapBounds (Bounds newMapBounds)
        {
            if (newMapBounds == null) return;
            
            _MapBounds = newMapBounds;
            UpdateBoundary();
            ClampCamera();
        }

        public void SetFollowCharacter (Transform transform)
        {
            _FollowCharacter = transform;
            
            if (_FollowCharacter == null) return;

            _Camera.orthographicSize = GameSettings.CameraMinSize;
            UpdateBoundary();
            ClampCamera();
        }
#endregion
#region Private Functions
        private void FollowCameraUpdate ()
        {
            Vector3 startPos = _CameraTransform.position;
            Vector3 endPos = _FollowCharacter.position;
            endPos.x += GameSettings.CameraOffSet.x;
            endPos.y += GameSettings.CameraOffSet.y;
 
            _CameraTransform.position = Vector3.Lerp(startPos, endPos, GameSettings.CameraDampening);
            ClampCamera();
        }

        private void DragCameraUpdate ()
        {
            if (Input.GetMouseButton(1))
            {
                _Difference = (_Camera.ScreenToWorldPoint(Input.mousePosition)) - _CameraTransform.position;

                if (!_IsDrag)
                {
                    _IsDrag = true;
                    _StartPosition = _Camera.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                _IsDrag = false;
            }

            if (_IsDrag)
            {
                _CameraTransform.position = _StartPosition - _Difference;
                ClampCamera();
            }
        }

        private void HandleCameraZoom ()
        {

        }

        private void UpdateBoundary ()
        {
            float h = _Camera.orthographicSize;
            float w = h * _Camera.aspect;

            float minX = _MapBounds.min.x + w - 0.1f;
            float maxX = _MapBounds.max.x - w + 0.1f;

            float minY = _MapBounds.min.y + h;
            float maxY = _MapBounds.max.y - h;

            _CameraBounds.SetMinMax
            (
                new Vector3(minX, minY, 0f),
                new Vector3(maxX, maxY, 0f)
            );
        }

        private void ClampCamera ()
        {
            _CameraTransform.position = new Vector3 
            (
                Mathf.Clamp(_CameraTransform.position.x, _CameraBounds.min.x, _CameraBounds.max.x),
                Mathf.Clamp(_CameraTransform.position.y, _CameraBounds.min.y, _CameraBounds.max.y),
                _CameraTransform.position.z
            );
        }
#endregion
    }
}