using System;
using System.Threading.Tasks;
using MovementAndAnimation;
using MonsterSystem;

namespace StateSystem
{
    public class StillState : BaseState
    {
        // Memebers
        private SpriteController _Controller;
        private MonsterAI _Context;

        private bool _StartWander;
        private DateTime _TimeNextWanderCheck;

        private Random rng;

        public StillState (SpriteController controller, MonsterAI context)
        {
            _Controller = controller;
            _Context = context;

            rng = new Random();
        } 
#region Inherited Functions
        public override void EnterState ()
        {
            _StartWander = false;

            _Controller.MoveHorizontal(0);
            _TimeNextWanderCheck = DateTime.Now.AddMilliseconds(rng.Next(1000, 3001));
        }
        
        public override void HandleState ()
        {
            if (_TimeNextWanderCheck.CompareTo(DateTime.Now) <= 0) WanderCheck();
        }
 
        public override void ExitState (){}
        
        public override Type CheckSwitchState ()
        {
            if (_Context.TargetTracking != null) return typeof(TrackingState);

            if (_StartWander) return typeof(WanderState);
            else return this.GetType();
        }
#endregion
#region Private Functions
        private void WanderCheck ()
        {
            // 50/50 For Wandering
            if (rng.NextDouble() <= 0.5)
                _StartWander = true;
            else // Randomize Wait time between 1 to 3 seconds
                _TimeNextWanderCheck = DateTime.Now.AddMilliseconds(rng.Next(1000, 3001));
        }
#endregion
    }
}