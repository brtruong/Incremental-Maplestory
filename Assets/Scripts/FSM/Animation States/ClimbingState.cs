using System;
using UnityEngine;
using MovementAndAnimation;

namespace StateSystem
{
    public class ClimbingState : BaseState
    {
            private SpriteController _Controller;
            private Vector2 _ClimbingVelocity;
            private Vector2 _RopeJumpVelocity;
            private Vector2 _RopePosition;

            public ClimbingState (SpriteController controller)
            {
                _Controller = controller;
                _RopePosition = new Vector2(0f, 0f);
                _ClimbingVelocity = new Vector2(0f, 0f);
                _RopeJumpVelocity = new Vector2(_Controller.JumpForce / 4, _Controller.JumpForce / 2);
            }

            public override void EnterState ()
            {
                _Controller.CanDoubleJump = true;
                _Controller.AnimatorSprite.Play("Climbing Idle");
                _Controller.Rb.bodyType = RigidbodyType2D.Kinematic;

                // Snap To Rope
                _RopePosition.x = _Controller.RopeCollision.bounds.center.x;
                _RopePosition.y = _Controller.transform.position.y;
                _Controller.transform.position = _RopePosition;
            }

            public override void HandleState ()
            {
                _ClimbingVelocity.y = _Controller.VerticalInput * _Controller.ClimbingSpeed;
                _Controller.Rb.velocity = _ClimbingVelocity;

                if (_Controller.VerticalInput != 0)
                    _Controller.AnimatorSprite.Play("Climbing");
                else
                    _Controller.AnimatorSprite.Play("Climbing Idle"); 
            }
            
            public override void ExitState ()
            {
                _Controller.Rb.bodyType = RigidbodyType2D.Dynamic;
            }

            public override Type CheckSwitchState ()
            {
                // Check If Dead
                if (_Controller.IsDead) return typeof(DeadState);

                // Check For Stagger
                if (_Controller.IsStaggered) return typeof(StaggerState);

                // Check for Rope Jump
                if (_Controller.JumpInput && _Controller.HorizontalInput != 0) return RopeJump();

                // Check if Still on Rope
                if (_Controller.RopeCollision != null) return this.GetType();

                // Check For Skill Use
                if (_Controller.QueuedSkill != null) return typeof(SkillState);

                // Check If Falling
                if (_Controller.GroundCollision == null) return typeof(FallingState);

                // Check For Jump Input
                if (_Controller.JumpInput) return typeof(JumpingState);

                // Check For Horizontal Movement
                if (_Controller.HorizontalInput != 0) return typeof(WalkingState);

                // Check For Prone
                if (_Controller.VerticalInput == -1) return typeof(ProneState);
                
                return typeof(IdleState);
            }

            private Type RopeJump ()
            {
                ExitState();
                _RopeJumpVelocity.x = _Controller.HorizontalInput * _Controller.JumpForce / 4;
                _Controller.Rb.AddForce(_RopeJumpVelocity, ForceMode2D.Impulse);

                if ((int)_Controller.SpriteDirection == _Controller.HorizontalInput) _Controller.FlipSprite();

                _Controller.Jump(false);
                return typeof(FallingState);
            }
    }
}