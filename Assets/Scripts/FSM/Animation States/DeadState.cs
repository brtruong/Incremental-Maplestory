using System;
using MovementAndAnimation;

namespace StateSystem
{
    public class DeadState : BaseState
    {
            private SpriteController _Controller;

            public DeadState (SpriteController controller)
            {
                _Controller = controller;
            }

            public override void EnterState ()
            {
                _Controller.AnimatorSprite.Play("Die");
            }

            public override void HandleState (){}
            
            public override void ExitState (){}

            public override Type CheckSwitchState ()
            {
                if (_Controller.IsDead) return this.GetType();
                else return typeof(IdleState);
            }
    }
}