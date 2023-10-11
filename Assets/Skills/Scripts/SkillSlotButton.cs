using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SkillSystem;

namespace UISystem
{
    public class SkillSlotButton : CustomButton
    {
        [SerializeField] private Image _IconImage;
        [SerializeField] private Text _NameText, _LevelText;
        [SerializeField] private Skill _Skill;

        public void Init (Skill newSkill)
        {
            _Skill = newSkill;
            _IconImage.sprite = _Skill.Base.Icon;
            _NameText.text = _Skill.Base.Name;
            _LevelText.text = _Skill.Level.ToString();
        }

        public void LevelUpSkill ()
        {
            _Skill.LevelUp();
            _LevelText.text = _Skill.Level.ToString();
        }
#region Inherited Functions
        protected override void HoverAction (PointerEventData data){}
        protected override void DragStartAction (PointerEventData data)
        {
            CursorUI.Instance.StartDrag(_IconImage.sprite);
        }

        protected override void DragEndAction (PointerEventData data)
        {
            foreach (GameObject g in data.hovered)
            {
                if (g.tag == "Quick Slot") g.GetComponent<QuickSlotButton>().SetSlot(_Skill);
            }
        }
#endregion
    }
}