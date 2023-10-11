using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class InventoryEquips
    {
        [SerializeField] private EquipType _Type;
        [SerializeField] private bool _IsFull; 
        [SerializeField] private List<EquipSlot> _EquipSlots;

        public EquipType Type => _Type;
        public List<EquipSlot> EquipSlots => _EquipSlots;

        public InventoryEquips (EquipType type)
        {
            _Type = type;
            _IsFull = false;
            _EquipSlots = new List<EquipSlot>();

            for (int i = 0; i < 30; i++)
                _EquipSlots.Add(new EquipSlot(_Type));
        }

        public void UpdateAllItems ()
        {
            foreach (EquipSlot equipSlot in _EquipSlots)
                if (!equipSlot.IsEmpty) equipSlot.Item.UpdateBase();
        }

        public bool AddItem (ItemEquip item)
        {
            if (_IsFull || item.EquipType != _Type) return false;

            foreach (EquipSlot equipSlot in _EquipSlots)
            {
                if (!equipSlot.IsEmpty) continue;

                ItemEquip returnItem = equipSlot.ChangeItem(item);
                return true;
            }

            _IsFull = true;
            return false;
        }
        
        public object CaptureSave ()
        {
            List<object> itemsData = new List<object>();
            foreach (EquipSlot slot in _EquipSlots)
            {
                if (slot.IsEmpty) itemsData.Add(null);
                else itemsData.Add(slot.Item.CaptureSave());
            }
            return new SaveData(){Type = _Type, IsFull = _IsFull, ItemsData = itemsData};
        }

        private struct SaveData
        {
            public EquipType Type;
            public bool IsFull;
            public List<object> ItemsData;
        }
    }
}