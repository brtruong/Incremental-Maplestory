using UnityEngine;
using SkillSystem;

namespace MonsterSystem
{
    [System.Serializable]
    public class Monster
    {
        // Memebers
        [SerializeField] private MonsterBase _Base;
        [SerializeField] private int _Level, _HP, _MaxHP, _MesoDropAmount;
        [SerializeField] private bool _IsDead;
        [SerializeField] private Skill _BasicAttack;

        public MonsterBase Base => _Base;
        public int Level => _Level;
        public int HP => _HP;
        public int MaxHP => _MaxHP;
        public int MesoDropAmount => _MesoDropAmount;
        public int PDR => _Base.PhysicalDamageReduction;
        public bool IsBoss => _Base.IsBoss;
        public bool IsDead => _IsDead;

        public Skill BasicAttack => _BasicAttack;

#region Public Functions
        public Monster (MonsterBase monsterBase, int level)
        {
            _Base = monsterBase;
            _Level = level;
            _IsDead = false;

            _BasicAttack = new Skill(_Base.BasicAttack);

            if (level <= 0) _IsDead = true;
            SetUp();
        }

        public void Damage (int value)
        {
            if (value <= 0) return;

            _HP -= value;

            if (_HP <= 0) _IsDead = true;
        }

        public int RollDamage ()
        {
            return Random.Range(_Level * 10, _Level * 20);
        }
#endregion
#region Private Functions
        private void SetUp ()
        {
            _MaxHP = _Level * 20;
            _HP = _MaxHP;
            _MesoDropAmount = _Level * 10;
        }
#endregion
    }
}