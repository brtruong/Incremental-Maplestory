using System;

namespace StateSystem
{
    public abstract class BaseState
    {
        public virtual void EnterState (){}
        public virtual void HandleState (){}
        public virtual void ExitState (){}
        public virtual Type CheckSwitchState (){return null;}
    }
}