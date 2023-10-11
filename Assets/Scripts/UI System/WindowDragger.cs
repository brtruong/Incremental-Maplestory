using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class WindowDragger : MonoBehaviour
    {
        public static WindowDragger Instance {get; private set;}
        private Vector2 _MouseOffset;
        private RectTransform _Window;

        private void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(this);    
        }

        public static void StartDrag (RectTransform transform) => Instance?.PrivateStartDrag(transform);
        public static void StopDrag () => Instance?.PrivateStopDrag();

        private void PrivateStartDrag (RectTransform transform)
        {
            if (transform == null) return;
            
            _Window = transform;
            _MouseOffset = (Vector2)Input.mousePosition - _Window.anchoredPosition;
            InvokeRepeating("Drag", 0f, 0.005f);
        }
        private void PrivateStopDrag () => CancelInvoke("Drag");
        private void Drag () => _Window.anchoredPosition = (Vector2)Input.mousePosition - _MouseOffset;
    }
}