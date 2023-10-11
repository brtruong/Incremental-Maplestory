using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using AudioSystem;

namespace UISystem
{
    public enum ClickType
    {
        LeftClick, RightClick, Draggable
    }

    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Custom Button Settings")]
        [SerializeField] private ClickType _ClickType;
        [SerializeField] private AudioClip _HoverSFX, _ClickSFX;
        [SerializeField] private bool _AllowCusorHover, _AllowCursorClick;
        [SerializeField] private bool _AllowAudioHover, _AllowAudioClick;
        [SerializeField] protected UnityEvent _ClickDownEvent, _ClickUpEvent;

        private PointerEventData.InputButton _ClickCheck;
        private bool _AllowOnPointerUp, _IsDragging;

        private delegate void VoidDelegate();
        private VoidDelegate _HandleHover, _PlayCursorHover, _PlayCursorClick;
        private VoidDelegate _PlayAudioHover, _PlayAudioClick;

        private void DoNothing (){}

        private delegate void PointerDelegate(PointerEventData data);
        private PointerDelegate _HandleOnPointerDown,  _HandleOnPointerUp;
        private PointerDelegate _HandleOnPointerEnter, _HandleOnPointerExit;

        private void Awake ()
        {
            _AllowOnPointerUp = _IsDragging = false;

            _PlayAudioHover = (_AllowAudioHover) ? delegate {AudioManager.Instance?.PlayAudio(_HoverSFX);} : DoNothing;
            _PlayAudioClick = (_AllowAudioClick) ? delegate {AudioManager.Instance?.PlayAudio(_ClickSFX);} : DoNothing;
            _PlayCursorClick = (_AllowCursorClick) ? delegate {CursorUI.Instance.PlayClick();} : DoNothing;

            if (_ClickType == ClickType.LeftClick)
            {
                _PlayCursorHover = (_AllowCusorHover) ? delegate {CursorUI.Instance.PlayHoverLMB();} : DoNothing;
                _ClickCheck = PointerEventData.InputButton.Left;
                _HandleOnPointerDown = OnPointerDownStandard;
                _HandleOnPointerUp = OnPointerUpStandard;
                _HandleOnPointerExit = OnPointerExitStandard;
            }
            else if (_ClickType == ClickType.RightClick)
            {
                _PlayCursorHover = (_AllowCusorHover) ? delegate {CursorUI.Instance.PlayHoverRMB();} : DoNothing;
                _ClickCheck = PointerEventData.InputButton.Right;
                _HandleOnPointerDown = OnPointerDownStandard;
                _HandleOnPointerUp = OnPointerUpStandard;
                _HandleOnPointerExit = OnPointerExitStandard;
            }
            else
            {
                _PlayCursorHover = (_AllowCusorHover) ? delegate {CursorUI.Instance.PlayHoverDrag();} : DoNothing;
                _ClickCheck = PointerEventData.InputButton.Left;
                _HandleOnPointerDown = OnPointerDownDrag;
                _HandleOnPointerUp = OnPointerUpDrag;
                _HandleOnPointerExit = OnPointerExitDrag;
            }
        }


        // On Pointer Down
        public void OnPointerDown (PointerEventData data) => _HandleOnPointerDown(data);
        private void OnPointerDownStandard (PointerEventData data)
        {
            if (data.button != _ClickCheck) return;

            _AllowOnPointerUp = true;
            _PlayAudioClick();
            _PlayCursorClick();
            ClickDownAction();
        }
        private void OnPointerDownDrag (PointerEventData data)
        {
            if (data.button != _ClickCheck) return;

            _IsDragging = true;
            _PlayAudioClick();

            DragStartAction(data);
        }

        // On Pointer Up
        public void OnPointerUp(PointerEventData data) => _HandleOnPointerUp(data);
        private void OnPointerUpStandard (PointerEventData data)
        {
            if (!_AllowOnPointerUp) return;

            _AllowOnPointerUp = false;
            // Check if it should be idle or hover
            CursorUI.Instance.PlayIdle();
            ClickUpAction();
        }
        private void OnPointerUpDrag (PointerEventData data)
        {
            _IsDragging = false;
            CursorUI.Instance.StopDrag();
            DragEndAction(data);
        }

        // On Pointer Enter
        public void OnPointerEnter (PointerEventData data)
        {
            _PlayCursorHover();
            _PlayAudioHover();
            HoverAction(data);
        }

        // On Pointer Exit
        public void OnPointerExit (PointerEventData data) => _HandleOnPointerExit(data);
        private void OnPointerExitStandard (PointerEventData data)
        {
            _AllowOnPointerUp = false;
            CursorUI.Instance.PlayIdle();
            ExitAction(data);
        }
        private void OnPointerExitDrag (PointerEventData data)
        {
            CursorUI.Instance.PlayIdle();
            if (_IsDragging) return;
            ExitAction(data);
        }

        protected virtual void ClickDownAction () => _ClickDownEvent?.Invoke();
        protected virtual void ClickUpAction () => _ClickUpEvent?.Invoke();
        protected virtual void DragStartAction (PointerEventData data) => CursorUI.Instance.StartDrag(null);
        protected virtual void DragEndAction (PointerEventData data){}
        protected virtual void HoverAction (PointerEventData data){}
        protected virtual void ExitAction (PointerEventData data){}
    }
}