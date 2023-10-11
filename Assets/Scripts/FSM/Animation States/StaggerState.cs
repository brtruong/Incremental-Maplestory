using System;
using MovementAndAnimation;

namespace StateSystem
{
    public class StaggerState : BaseState
    {
            private SpriteController _Controller;

            public StaggerState (SpriteController controller)
            {
                _Controller = controller;
            }

            public override void EnterState ()
            {
                _Controller.AnimatorSprite.Play("Stagger");
            }

            public override void HandleState (){}
            
            public override void ExitState (){}

            public override Type CheckSwitchState ()
            {
                // Check If Dead
                if (_Controller.IsDead) return typeof(DeadState);
                
                if (_Controller.IsStaggered) return this.GetType();
                else return typeof(IdleState);
            }
    }
}