using System.Collections.Generic;
using UnityEngine;

using SkillSystem;

namespace UISystem
{
    public class QuickSlotManager : MonoBehaviour
    {
        public static QuickSlotManager Instance {get; private set;}

        [SerializeField] private GameObject _QuickSlotWindow;
        [SerializeField] private QuickSlotButton[] _QuickSlots;
        private Dictionary <string, QuickSlotButton> _QuickSlotTable;

        private void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(this);

            _QuickSlotTable = new Dictionary<string, QuickSlotButton>();
            ResetQuickSlots();
        }

        private void ResetQuickSlots ()
        {
            foreach (QuickSlotButton quickSlot in _QuickSlots)
                _QuickSlotTable.Add(quickSlot.Keybind, quickSlot);
        }

        public void UpdateQuickSlot (string keybind, Skill skill)
        {
            if (!_QuickSlotTable.ContainsKey(keybind)) return;

            _QuickSlotTable[keybind].SetSlot(skill);
        }

        public void ResetSkillQuickSlots ()
        {
            foreach (QuickSlotButton quickSlot in _QuickSlots)
                if (quickSlot.HoldingSkill) quickSlot.Clear();
        }
    }
}