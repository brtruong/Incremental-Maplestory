using System;
using MovementAndAnimation;

namespace StateSystem
{
    public class ProneState : BaseState
    {
            private SpriteController _Controller;

            public ProneState (SpriteController controller)
            {
                _Controller = controller;
            }

            public override void EnterState ()
            {
                _Controller.CanDoubleJump = true;
                _Controller.AnimatorSprite.Play("Prone");
            }

            public override void HandleState (){}
            
            public override void ExitState (){}

            public override Type CheckSwitchState ()
            {
                // Check If Dead
                if (_Controller.IsDead) return typeof(DeadState);
                
                // Check For Stagger
                if (_Controller.IsStaggered) return typeof(StaggerState);

                // Check For Skill Use
                if (_Controller.QueuedSkill != null) return typeof(SkillState);
                
                // Check For Prone Jump
                //if (_Controller.VerticalInput == -1 && _Controller.JumpInput) return typeof(JumpingState);

                // Check For Jumping
                if (_Controller.JumpInput) return typeof(JumpingState);

                // Check For Walking
                if (_Controller.HorizontalInput != 0) return typeof(WalkingState);

                // Check If Still Proned               
                if (_Controller.VerticalInput == -1) return this.GetType();

                return typeof(IdleState);
            }

            private void ProneJump ()
            {

            }
    }
}