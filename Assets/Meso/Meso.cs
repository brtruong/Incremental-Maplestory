using System;
using UnityEngine;

namespace MesoSystem
{
    [System.Serializable]
    public class Meso
    {
        [SerializeField] private int _Amount;

        public int Amount => _Amount;

        public Meso (int startAmount)
        {
            _Amount = (startAmount > 0) ? startAmount : 0;
        }

        public bool Add (int amount)
        {
            if (amount < 0) return false;
        
            _Amount += amount;
            return true;
        }

        public bool Subtract (int amount)
        {
            if (amount < 0 || amount > _Amount) return false;

            _Amount -= amount;
            return true;
        }
    }

    public class OnMesoUpdateArgs : EventArgs
    {
        public int Meso;
    }
}