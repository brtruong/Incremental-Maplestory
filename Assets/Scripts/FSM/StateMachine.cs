using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateSystem
{
    public class StateMachine
    {
        private Dictionary<Type, BaseState> _StateTable;
        private BaseState _CurrentState;

        public BaseState State => _CurrentState;

        public StateMachine (Dictionary<Type, BaseState> states)
        {
            _StateTable = states;
            _CurrentState = GetFirstState();
            _CurrentState.EnterState();
        }

        public void Update ()
        {
            if (_CurrentState == null)
                _CurrentState = GetFirstState();

            _CurrentState.HandleState();

            Type newState = _CurrentState.CheckSwitchState();
            if (newState != null && newState != _CurrentState.GetType())
                SwitchToNewState(newState);
        }

        private void SwitchToNewState (Type newState)
        {
            _CurrentState.ExitState();
            _CurrentState = _StateTable[newState];
            _CurrentState?.EnterState();
        }

        private BaseState GetFirstState ()
        {
            foreach (BaseState b in _StateTable.Values)
                return b;
            return null;
        }

    }
}