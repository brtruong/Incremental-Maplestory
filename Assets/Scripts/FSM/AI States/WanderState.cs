using System;
using System.Threading.Tasks;
using MovementAndAnimation;
using MonsterSystem;

namespace StateSystem
{
    public class WanderState : BaseState
    {
        // Memebers
        private SpriteController _Controller;
        private MonsterAI _Context;

        private bool _ContinueWander;
        private int _Direction;
        private float _TargetX, _Min, _Max;
        private DateTime _TimeEndWander;

        private Random rng;

        public WanderState (SpriteController controller, MonsterAI context)
        {
            _Controller = controller;
            _Context = context;

            rng = new Random();
        } 
#region Inherited Functions
        public override void EnterState ()
        {
            _ContinueWander = true;
            UpdateBounds();
            PickDirection();
            PickDistance();
            _TimeEndWander = DateTime.Now.AddMilliseconds(3000);
            _Controller.MoveHorizontal(_Direction);
        }
        
        public override void HandleState ()
        {
            if (Math.Abs(_TargetX - _Controller.transform.position.x) <= 0.05f)
                _ContinueWander = false;
        }

        public override void ExitState (){}
        
        public override Type CheckSwitchState ()
        {
            if (_Context.TargetTracking != null) return typeof(TrackingState);

            if (_ContinueWander) return this.GetType();
            else return typeof(StillState);
        }
#endregion
#region Private Functions
        private void UpdateBounds ()
        {
            _Min = _Controller.GroundCollision.bounds.min.x;
            _Max = _Controller.GroundCollision.bounds.max.x;
        }

        private void PickDirection ()
        {
            // Check if Close To Edge
            if (Math.Abs(_Min - _Controller.transform.position.x) <= 0.5f)
            {
                // Close to Left Edge 80% Move Right
                _Direction = (rng.NextDouble() <= 0.8) ? 1 : -1;
            }
            else if (Math.Abs(_Max - _Controller.transform.position.x) <= 0.5f)
            {
                // Close to Right Edge 80% Move Left
                _Direction = (rng.NextDouble() <= 0.8) ? -1 : 1;
            }
            else
            {
                // Not Close to Edge 50/50 direction
                _Direction = (int)_Controller.SpriteDirection;
                _Direction *= (rng.NextDouble() <= 0.5) ? -1 : 1;
            }
        }

        private void PickDistance ()
        {
            _TargetX = _Controller.transform.position.x + (rng.Next(1, 3) + (float)rng.NextDouble()) * _Direction;
            _TargetX = Math.Clamp(_TargetX, _Min, _Max);
        }
#endregion
    }
}