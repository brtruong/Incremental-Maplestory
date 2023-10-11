using System;
using MovementAndAnimation;

namespace StateSystem
{
    public class IdleState : BaseState
    {
            private SpriteController _Controller;

            public IdleState (SpriteController controller)
            {
                _Controller = controller;
            }

            public override void EnterState ()
            {
                _Controller.CanDoubleJump = true;
                _Controller.AnimatorSprite.Play("Idle");
                _Controller.Rb.sharedMaterial = GameSettings.FrictionIdle;
            }

            public override void HandleState () {}
            
            public override void ExitState (){}

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
                
                // Check For Falling
                if (_Controller.GroundCollision == null) return typeof(FallingState);

                // Check For Jumping
                if (_Controller.JumpInput) return typeof(JumpingState);

                // Check For Walking
                if (_Controller.HorizontalInput != 0) return typeof(WalkingState);

                // Check For Prone
                if (_Controller.VerticalInput == -1) return typeof(ProneState);

                return this.GetType();
            }
    }
}