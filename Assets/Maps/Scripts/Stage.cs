using System;
using UnityEngine;

namespace MapSystem
{
    [System.Serializable]
    public class Stage
    {
        [SerializeField] private int _ID;
        [SerializeField] private int _Level;
        [SerializeField] private int _KillCount;
        [SerializeField] private int _KillRequirement;

        public int ID => _ID;
        public int Level => _Level;
        public int KillCount => _KillCount;
        public int KillRequirement => _KillRequirement;

        public Stage ()
        {
            _ID = 1; 
            _Level = 1;
            _KillCount = 0;
            _KillRequirement = 300;
        }

        public bool IncrementKillCount ()
        {
            _KillCount++;
            
            if (_KillCount < _KillRequirement) return false;
        
            _KillCount = 0;
            _Level++;

            return true;
        }
    }

    public class OnStageUpdateArgs : EventArgs
    {
        public int Level;
        public int KillCount;
        public int KillRequirement;
    }
} 