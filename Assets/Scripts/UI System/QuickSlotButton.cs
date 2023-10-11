using UnityEngine;
using UnityEngine.UI;

using SkillSystem;

namespace UISystem
{
    public class QuickSlotButton : CustomButton
    {
        [SerializeField] private Image _IconImage;
        [SerializeField] private Image _KeybindImage;
        [SerializeField] private string _Keybind; 

        private Skill _Skill;

        public bool HoldingSkill {get; private set;}
        public string Keybind => _Keybind;

        public void SetSlot (Skill newSkill)
        {
            // Remove Old Skill Keybind
            if (_Skill != null) Clear();

            _Skill = newSkill;
            _Skill.SetKeybind(_Keybind);
            _IconImage.sprite = _Skill.Base.Icon;
            _IconImage.enabled = true;

            HoldingSkill = true;
            _Skill.OnUpdate += HandleSkillUpdate;            
        }

        public void RemoveSkill ()
        {
            Clear();
            if (_Skill != null) _Skill.SetKeybind("None");
        }

        public void Clear ()
        {
            HoldingSkill = false;
            _IconImage.enabled = false;

            if (_Skill != null) _Skill.OnUpdate -=HandleSkillUpdate;
        }


        private void HandleSkillUpdate (Skill skill)
        {
            if (skill.Keybind != _Keybind)
            {
                _IconImage.enabled = false;
                skill.OnUpdate -= HandleSkillUpdate;
            }
        }
        
    }
}