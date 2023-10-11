using System;
using UnityEngine;
using MovementAndAnimation;

namespace StateSystem
{
    public class WalkingState : BaseState
    {
            private SpriteController _Controller;
            private Vector2 _Velocity;
            private RaycastHit2D _GroundHit;

            public WalkingState (SpriteController controller)
            {
                _Controller = controller;
                _Velocity = new Vector2(0f, 0f);
            }

            public override void EnterState ()
            {
                _Controller.CanDoubleJump = true;
                _Controller.AnimatorSprite.Play("Walking");
                _Controller.Rb.sharedMaterial = GameSettings.FrictionMove;
            }

            public override void HandleState ()
            {
                if ((int)_Controller.SpriteDirection == _Controller.HorizontalInput) _Controller.FlipSprite();
                
                _GroundHit = Physics2D.Raycast(_Controller.transform.position, Vector2.down, 0.5f, GameSettings.GroundLayer);

                if (_GroundHit)
                {
                    _Velocity = -Vector2.Perpendicular(_GroundHit.normal.normalized);
                    _Velocity *= _Controller.HorizontalInput * _Controller.MovementSpeed;
                    _Controller.Rb.velocity = _Velocity;
                }
                else
                {
                    // Walk Off Edge
                    _Velocity.y = 0f;
                    _Velocity.x = _Controller.HorizontalInput * _Controller.MovementSpeed;
                    _Controller.Rb.velocity = _Velocity;
                }
            }
            
            public override void ExitState () => _Controller.Rb.sharedMaterial = GameSettings.FrictionIdle;

            public override Type CheckSwitchState ()
            {
                // Check If Dead
                if (_Controller.IsDead) return typeof(DeadState);
                
                // Check For Stagger
                if (_Controller.IsStaggered) return typeof(StaggerState);

                // Check For Skill Use
                if (_Controller.QueuedSkill != null) return typeof(SkillState);

                // Check To Climb Rope
                if (_Controller.RopeCollision != null && _Controller.VerticalInput != 0) return typeof(ClimbingState);

                // Check For Jump
                if (_Controller.JumpInput) return typeof(JumpingState);

                // Check For Falling
                if (_Controller.GroundCollision == null) return typeof(FallingState);

                // Check For Idle or Walking
                if (_Controller.HorizontalInput == 0) return typeof(IdleState);

                return this.GetType();
            }
    }
}