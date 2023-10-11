using System;
using UnityEngine;
using MovementAndAnimation;
using SkillSystem;

namespace StateSystem
{
    public class FallingState : BaseState
    {
            private SpriteController _Controller;
            private Vector2 _DoubleJumpVector, _Velocity;
            private SkillBase _DoubleJumpSkill;

            public FallingState (SpriteController controller)
            {
                _Controller = controller;

                _DoubleJumpSkill = null;
                _DoubleJumpVector = new Vector2 (_Controller.JumpForce, _Controller.JumpForce / 2);
                _Velocity = Vector2.zero;
            }

            public void SetDoubleJump (SkillBase doubleJumpSkill)
            {
                _DoubleJumpSkill = doubleJumpSkill;
            }

            public override void EnterState ()
            {
                _Controller.AnimatorSprite.Play("Jumping");
            }

            public override void HandleState ()
            {
                if ((int)_Controller.SpriteDirection == _Controller.HorizontalInput) _Controller.FlipSprite();

                if (_Controller.JumpInput) DoubleJump();
            }
            
            public override void ExitState (){}

            public override Type CheckSwitchState ()
            {
                // Check If Dead
                if (_Controller.IsDead) return typeof(DeadState);
                
                // Check For Stagger
                if (_Controller.IsStaggered) return typeof(StaggerState);

                // Check For Skill Use
                if (_Controller.QueuedSkill != null) return typeof(SkillState);

                // Check To ClimbRope
                if (_Controller.VerticalInput != 0 && _Controller.RopeCollision) return typeof(ClimbingState);

                // Check If Still Falling
                if (_Controller.GroundCollision == null) return this.GetType();

                // Check For Jump Input
                if (_Controller.JumpInput) return typeof(JumpingState);

                // Check For Horizontal Movement
                if (_Controller.HorizontalInput != 0) return typeof(WalkingState);

                // Check For Prone
                if (_Controller.VerticalInput == -1) return typeof(ProneState);

                return typeof(IdleState);
            }

            private void DoubleJump ()
            {
                if (!_Controller.AllowDoubleJump || !_Controller.CanDoubleJump) return;

                _Controller.CanDoubleJump = false;
                _Controller.Jump(false);
                _Controller.QueueSkill(_DoubleJumpSkill);

                // Update Velocity
                _Velocity.x = 0f;
                _Velocity.y = _Controller.Rb.velocity.y;
                _Controller.Rb.velocity = _Velocity;

                _DoubleJumpVector.x = -1 * (int)_Controller.SpriteDirection * _Controller.JumpForce;
                _Controller.Rb.AddForce(_DoubleJumpVector, ForceMode2D.Impulse);
            }
    }
}