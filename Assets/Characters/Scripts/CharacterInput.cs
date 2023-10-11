using UnityEngine;
using UnityEngine.InputSystem;
using MovementAndAnimation;

namespace CharacterSystem
{
    public class CharacterInput : MonoBehaviour
    {
        // Memebers
        [Header("Settings")]
        [SerializeField] private SpriteController _SpriteController;
        [SerializeField] private CharacterBehaviour _CharacterBehaviour;

        private bool _Active;
        public void Activate () => _Active = true;
        public void Deactivate () => _Active = false;

        private void Start ()
        {
            GameSettings.KeyInput.Player.Jump.performed += HandlePerformedJumpInput;
            GameSettings.KeyInput.Player.Jump.canceled += HandleCanceledJumpInput;

            GameSettings.KeyInput.Player.Move.performed += HandleMoveInput;
            GameSettings.KeyInput.Player.Move.canceled += HandleMoveInput;

            GameSettings.KeyInput.Player.KeyInputs.performed += HandlePerformedKeyInputs;
            GameSettings.KeyInput.Player.KeyInputs.canceled += HandleCanceledKeyInputs;
        }

        private void OnDisable ()
        {
            GameSettings.KeyInput.Player.Jump.performed -= HandlePerformedJumpInput;
            GameSettings.KeyInput.Player.Jump.canceled -= HandleCanceledJumpInput;

            GameSettings.KeyInput.Player.Move.performed += HandleMoveInput;
            GameSettings.KeyInput.Player.Move.canceled -= HandleMoveInput;

            GameSettings.KeyInput.Player.KeyInputs.performed -= HandlePerformedKeyInputs;
            GameSettings.KeyInput.Player.KeyInputs.canceled -= HandleCanceledKeyInputs;
        }

        private void HandlePerformedJumpInput (InputAction.CallbackContext context)
        {
            if (!_Active) return;

            _SpriteController.Jump(true);
        }

        private void HandleCanceledJumpInput (InputAction.CallbackContext context)
        {
            if (!_Active) return;

            _SpriteController.Jump(false);
        }

        private void HandleMoveInput (InputAction.CallbackContext context)
        {
            if (!_Active) return;

            Vector2 move = context.ReadValue<Vector2>();
            _SpriteController.MoveHorizontal((int)move.x);
            _SpriteController.MoveVertical((int)move.y);
        }

        private void HandlePerformedKeyInputs (InputAction.CallbackContext context)
        {
            if (!_Active) return;

            _CharacterBehaviour.UseSkill(context.control.name);
        }

        private void HandleCanceledKeyInputs (InputAction.CallbackContext context)
        {
            if (!_Active) return;
        }
    }
}