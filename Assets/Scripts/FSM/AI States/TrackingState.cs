using System;
using MovementAndAnimation;
using MonsterSystem;

namespace StateSystem
{
    public class TrackingState : BaseState
    {
        // Memebers
        private SpriteController _Controller;
        private MonsterAI _Context;

        private int _Direction;

        public TrackingState (SpriteController controller, MonsterAI context)
        {
            _Controller = controller;
            _Context = context;
        } 
#region Inherited Functions
        public override void EnterState ()
        {
            GetDirection();
            _Controller.MoveHorizontal(_Direction);
        }
        
        public override void HandleState ()
        {
            GetDirection();
            _Controller.MoveHorizontal(_Direction);
        }

        public override void ExitState ()
        {
            _Controller.MoveHorizontal(0);
        }
        
        public override Type CheckSwitchState ()
        {
            if (_Context.TargetInRange) return typeof(AttackingState);

            if (_Context.TargetTracking == null) return typeof(WanderState);
            else return this.GetType();
        }
#endregion
#region Private Function
        private void GetDirection ()
        {
            // Check Which Side Tracked Target is On
            if (_Context.TargetTracking?.transform.position.x < _Controller.transform.position.x)
                _Direction = -1;    // On Left Side
            else
                _Direction = 1;     // On Right Side
        }
#endregion
    }
}