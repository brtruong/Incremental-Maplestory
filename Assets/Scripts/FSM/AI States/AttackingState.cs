using System;
using MovementAndAnimation;
using MonsterSystem;

namespace StateSystem
{
    public class AttackingState : BaseState
    {
        // Memebers
        private SpriteController _Controller;
        private MonsterAI _Context;
        private MonsterBehaviour _MonsterBehaviour;

        private int _Direction;

        public AttackingState (SpriteController controller, MonsterAI context, MonsterBehaviour monsterBehaviour)
        {
            _Controller = controller;
            _Context = context;
            _MonsterBehaviour = monsterBehaviour;
        } 
#region Inherited Functions
        public override void EnterState (){}
        
        public override void HandleState ()
        {
            GetDirection();
            _MonsterBehaviour.Attack();
        }

        public override void ExitState (){}
        
        public override Type CheckSwitchState ()
        {
            if (_Context.TargetInRange) return this.GetType();
            else return typeof(TrackingState);
        }
#endregion
#region  Private Functions
        private void GetDirection ()
        {
            // Check Which Side Tracked Target is On
            if (_Context.TargetInRange?.transform.position.x < _Controller.transform.position.x)
                _Direction = -1;    // On Left Side
            else
                _Direction = 1;     // On Right Side

            if ((int)_Controller.SpriteDirection == _Direction) _Controller.FlipSprite();
        }
#endregion
    }
}