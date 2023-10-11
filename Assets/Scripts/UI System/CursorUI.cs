using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateSystem;

namespace UISystem
{
    public enum CursorState
    {
        Idle, HoverRMB, HoverLMB, HoverDrag, Click, Drag, StopDrag
    }

    public class CursorUI : MonoBehaviour
    {
        // Memebers
        public static CursorUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private bool _DefaultCursorVisible;
        [SerializeField] private Animator _SpriteAnimator;
        [SerializeField] private Image _ImageHeld;
        [SerializeField] private Sprite _SpriteNothing;

        private Camera _Camera;
        private CursorState _CurrentState, _NextState;
#region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);

            Cursor.visible = _DefaultCursorVisible;
        }

        private void Start ()
        {
            _Camera = Camera.main;
            _CurrentState = CursorState.Idle;
        }

        private void Update ()
        {
            transform.position = Input.mousePosition;
            UpdateState(_NextState);
        }
#endregion
#region Public Functions
        public void PlayIdle () => _NextState = CursorState.Idle;
        public void PlayHoverRMB () => _NextState = CursorState.HoverRMB;
        public void PlayHoverLMB () => _NextState = CursorState.HoverLMB;
        public void PlayHoverDrag () => _NextState = CursorState.HoverDrag;
        public void PlayClick () => _NextState = CursorState.Click;
        public void StartDrag (Sprite spriteDragged)
        {
            if (_CurrentState == CursorState.Drag) return;
            
            _CurrentState = CursorState.Drag;
            _SpriteAnimator.Play("Drag");
            
            if (spriteDragged != null) _ImageHeld.sprite = spriteDragged;
        }
        public void StopDrag ()
        {
            _ImageHeld.sprite = _SpriteNothing;
            _CurrentState = CursorState.StopDrag;
            UpdateState(_NextState);
        }
#endregion
#region Private Functions
        private void UpdateState (CursorState nextState)
        {
            if (_CurrentState == nextState || _CurrentState == CursorState.Drag) return;

            _CurrentState = nextState;
            switch (nextState)
            {
                case CursorState.Idle:
                    _SpriteAnimator.Play("Idle");
                    break;
                
                case CursorState.HoverRMB:
                    _SpriteAnimator.Play("RMB Hover");
                    break;

                case CursorState.HoverLMB:
                    _SpriteAnimator.Play("LMB Hover");
                    break;

                case CursorState.HoverDrag:                
                    _SpriteAnimator.Play("Drag Hover");
                    break;

                case CursorState.Click:
                    _SpriteAnimator.Play("Click");
                    break;

                case CursorState.Drag:
                    _SpriteAnimator.Play("Drag");
                    break;

                default:
                    _SpriteAnimator.Play("Idle");
                    break;
            }
        }
#endregion
    }
}