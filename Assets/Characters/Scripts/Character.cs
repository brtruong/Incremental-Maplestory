using System;
using System.Collections.Generic;
using UnityEngine;

using MonsterSystem;
using InventorySystem;
using SkillSystem;

namespace CharacterSystem
{
    [System.Serializable]
    public class Character
    {
        public delegate void Delegate(Character character);
        public event Delegate OnUpdate, OnStatsUpdate;

        // Members
        [SerializeField] private int _ID;
        [SerializeField] private CharacterBase _Base;
        [SerializeField] private Equipment _Equipment;
        [SerializeField] private List<Skill> _Skills;
        [SerializeField] private Stats _BaseStats, _EquipmentStats, _TotalStats;
        [SerializeField] private int _Level, _HP, _MP;
        [SerializeField] private int _UpperDMG, _LowerDMG;

        private const int LEVEL_UP_STATS = 5;
        private Transform _Transform;
        [SerializeField] private float _PosX, _PosY;

#region Getters
        public int ID => _ID;
        public string Name => _Base.Name;
        public CharacterBase Base => _Base;
        public Equipment Equipment => _Equipment;
        public List<Skill> Skills => _Skills;
        public Stats BaseStats => _BaseStats;
        public Stats EquipmentStats => _EquipmentStats;
        public Stats TotalStats => _TotalStats;
        public int Level => _Level;
        public int HP => _HP;
        public int MP => _MP;
        public int UpperDMG => _UpperDMG;
        public int LowerDMG => _LowerDMG;

        public float PosX => _PosX;
        public float PosY => _PosY;
#endregion
#region Public Functions
        public Character (CharacterBase characterBase)
        {
            _Base = characterBase;
            _ID = _Base.ID;

            _BaseStats = new Stats();
            _EquipmentStats = new Stats();
            _TotalStats = new Stats();
            
            _Equipment = new Equipment();
            _Equipment.UpdateTotalStats();
            _Equipment.OnUpdate += HandleEquipmentUpdate;
            
            _Skills = new List<Skill>();
            _Level = 0;
            _BaseStats.ATT = _BaseStats.MATT = 5;
            _BaseStats.PctCrt = 5;
            _BaseStats.PctCrtDMG = 50;
            _BaseStats.PctMastery = 20;
            _Transform = null;
            _PosX = _PosY = -1f;

            foreach (SkillBase s in _Base.SkillBases)
                _Skills.Add(new Skill(s));
        }

        public void LevelUp ()
        {
            _Level++;
            _HP += 100;
            _MP += 10;
            _BaseStats.MaxHP += 100;
            _BaseStats.MaxMP += 10;
            _BaseStats.STR += LEVEL_UP_STATS;
            _BaseStats.DEX += LEVEL_UP_STATS;
            _BaseStats.INT += LEVEL_UP_STATS;
            _BaseStats.LUK += LEVEL_UP_STATS;
            
            UpdateStats();
        }

        public bool Damage (int value)
        {
            _HP -= value;
            _HP = Mathf.Clamp(_HP, 0, _TotalStats.MaxHP);
            OnUpdate?.Invoke(this);
            return (_HP == 0) ? true : false;
        }

        public void RecoverHP (int value)
        {
            _HP += value;
            _HP = Mathf.Clamp(_HP, 0, _TotalStats.MaxHP);
            OnUpdate?.Invoke(this);
        }

        public void RecoverMP (int value)
        {
            _MP += value;
            _MP = Mathf.Clamp(_MP, 0, _TotalStats.MaxMP);
            OnUpdate?.Invoke(this);
        }

        public (int, bool) RollDamage (bool isBoss, int PDR)
        {
            int damage = UnityEngine.Random.Range(_LowerDMG, _UpperDMG + 1);
            bool isCrit = (UnityEngine.Random.Range(0, 101) > _TotalStats.PctCrt) ? false : true;
            
            float damageMultiplier = (isBoss) ? 1 + ((_TotalStats.PctBossDMG + _TotalStats.PctDMG) / 100) : 1 + (_TotalStats.PctDMG / 100);
            damageMultiplier *= (isCrit) ? 1 + (_TotalStats.PctCrtDMG / 100) : 1;

            // needs to add PDR calculation
            damage = (int) (damage * damageMultiplier);

            return (damage, isCrit);
        }
#endregion
#region Character Manager Functions
        public void UpdateBase ()
        {
            _Base = DatabaseManager.GetCharacterBase(_ID);

            foreach (Skill s in _Skills)
                s.UpdateBase();

            AddMissingSkills();

            _Equipment.OnUpdate += HandleEquipmentUpdate;
        }
        
        public void SavePosition ()
        {
            if (_Transform == null) return;

            _PosX = _Transform.position.x;
            _PosY = _Transform.position.y + 0.05f;
        }
        
        public void SetTransform (Transform transform)
        {
            if (transform == null) return;
            _Transform = transform;
        }
#endregion
#region Private Functions
        private void AddMissingSkills ()
        {
            List<Skill> temp = new List<Skill>();
            foreach (Skill skill in _Skills)
                temp.Add(skill);

            _Skills.Clear();

            foreach (SkillBase skill in _Base.SkillBases)
            {
                bool exists = false;
 
                for (int i = 0; i < temp.Count; i++) // Check if Skill does not exists
                {
                    if (skill.ID != temp[i].Base.ID) continue;

                    // Skill Already Exists Add it back
                    _Skills.Add(temp[i]);
                    temp.RemoveAt(i);
                    exists = true;
                    break;
                }

                if (!exists) _Skills.Add(new Skill(skill)); // Skill doesn't exist create it
            }
        }

        private void HandleEquipmentUpdate (Stats totalEquipmentStats)
        {
            _EquipmentStats = totalEquipmentStats;
            UpdateStats();
        }

        private void UpdateStats ()
        {
            _TotalStats = Stats.Sum(new Stats[]{_BaseStats, _EquipmentStats});
            UpdateDamage();
        }

        private void UpdateDamage ()
        {
            _UpperDMG = (int)Mathf.Round(_TotalStats.GetStat(_Base.MainStat) * _TotalStats.ATT * (1 + _TotalStats.PctDMG/100)); 
            _LowerDMG = (int)Mathf.Round(_UpperDMG * _TotalStats.PctMastery/100);

            OnUpdate?.Invoke(this);
            OnStatsUpdate?.Invoke(this);
        }
#endregion
    }
}