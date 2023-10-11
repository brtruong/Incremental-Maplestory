using System;
using UnityEngine;

namespace SkillSystem
{
    [System.Serializable]
    public class Skill
    {
        // Members
        [SerializeField] private int _ID;
        [SerializeField] private SkillBase _Base;
        [SerializeField] private int _Level;
        [SerializeField] private string _Keybind;

        public delegate void Delegate(Skill skill);
        public event Delegate OnUpdate;

        public int ID => _ID;
        public SkillBase Base => _Base;
        public int Level => _Level;
        public string Keybind => _Keybind;

        public Skill (SkillBase skillBase)
        {
            _ID = skillBase.ID;
            _Base = skillBase;
            _Level = 0;
            _Keybind = "None";
        }

        public void UpdateBase () => _Base = DatabaseManager.GetSkillBase(_ID);
        public void SetKeybind (string newKeybind)
        {
            _Keybind = newKeybind;
            OnUpdate?.Invoke(this);
        }
        public void LevelUp ()
        {
            _Level++;
            OnUpdate?.Invoke(this);
        }
    }

    public class OnUseSkillArgs : EventArgs
    {
        public SkillBase Base;
    }
}