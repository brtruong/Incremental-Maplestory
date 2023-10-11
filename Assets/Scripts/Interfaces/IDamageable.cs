using System;
using UnityEngine;

namespace DamageSystem
{
    interface IDamageable
    {
        public event EventHandler<OnDamageArgs> OnDamage;
        public event EventHandler<OnStaggerArgs> OnStagger;

        public void Damage (DamageLines damageLines, string hitEffect);
    }

    public class OnStaggerArgs : EventArgs
    {
        public float StaggerTime;
    }

    public class OnDamageArgs : EventArgs
    {
        public int HP;
        public int MaxHP;
        public bool IsDead;
        public string DamageEffect;
        public DamageLines DamageLines;
    }
}