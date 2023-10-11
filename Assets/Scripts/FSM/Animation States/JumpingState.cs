using System;
using UnityEngine;
using MovementAndAnimation;

namespace StateSystem
{
    public class JumpingState : BaseState
    {
            private SpriteController _Controller;
            private Vector2 _Velocity, _JumpVector;

            public JumpingState (SpriteController controller)
            {
                _Controller = controller;
                _Velocity = new Vector2(0f, 0f);
                _JumpVector = new Vector2(0, _Controller.JumpForce);
            }

            public override void EnterState ()
            {
                _Controller.AnimatorSprite.Play("Jumping");

                // Update Velocity
                _Velocity.y = 0f;
                _Velocity.x = _Controller.Rb.velocity.x;
                _Controller.Rb.velocity = _Velocity;
                _Controller.Rb.AddForce(_JumpVector, ForceMode2D.Impulse);
            }

            public override void HandleState (){}
            public override void ExitState (){}

            public override Type CheckSwitchState ()
            {
                // Check For Skill Use
                if (_Controller.QueuedSkill != null) return typeof(SkillState);

                // Check To ClimbRope
                if (_Controller.VerticalInput != 0 && _Controller.RopeCollision) return typeof(ClimbingState);

                // Check Jump -> Falling
                if (!_Controller.JumpInput) return typeof(FallingState);
                
                // Still Jump / Falling
                if (_Controller.GroundCollision == null || _Controller.Rb.velocity.y > 0.5) return this.GetType();

                // Jump Walking 
                if (_Controller.HorizontalInput != 0) return typeof(WalkingState);
                else return typeof(IdleState);
            }
    }
}