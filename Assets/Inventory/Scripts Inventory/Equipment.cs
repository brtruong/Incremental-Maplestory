using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Equipment
    {
        public static int Size = 20;

        public EquipSlot this[int idx] {get => _Equips[idx];}
        public delegate void HandleUpdate(Stats totalStats);
        public event HandleUpdate OnUpdate;

        [SerializeField] private EquipSlot[] _Equips;
        
        private Dictionary<EquipType, EquipSlot> _EquipTable;
        private Stats _TotalStats;

        public Equipment ()
        {
            _Equips = new EquipSlot[20];
            _Equips[0] = new EquipSlot(EquipType.Ring);
            _Equips[1] = new EquipSlot(EquipType.Hat);
            _Equips[2] = new EquipSlot(EquipType.Emblem);
            _Equips[3] = new EquipSlot(EquipType.Ring);
            _Equips[4] = new EquipSlot(EquipType.Pendant);
            _Equips[5] = new EquipSlot(EquipType.FaceAcc);
            _Equips[6] = new EquipSlot(EquipType.EyeAcc);
            _Equips[7] = new EquipSlot(EquipType.Earrings);
            _Equips[8] = new EquipSlot(EquipType.Ring);
            _Equips[9] = new EquipSlot(EquipType.Weapon);
            _Equips[10] = new EquipSlot(EquipType.Top);
            _Equips[11] = new EquipSlot(EquipType.Shoulder);
            _Equips[12] = new EquipSlot(EquipType.SubWeapon);
            _Equips[13] = new EquipSlot(EquipType.Ring);
            _Equips[14] = new EquipSlot(EquipType.Belt);
            _Equips[15] = new EquipSlot(EquipType.Bottom);
            _Equips[16] = new EquipSlot(EquipType.Gloves);
            _Equips[17] = new EquipSlot(EquipType.Cape);
            _Equips[18] = new EquipSlot(EquipType.Ring);
            _Equips[19] = new EquipSlot(EquipType.Shoes);

            _EquipTable = new Dictionary<EquipType, EquipSlot>();
            _EquipTable.Add(EquipType.Hat, _Equips[1]);
            _EquipTable.Add(EquipType.Emblem, _Equips[2]);
            _EquipTable.Add(EquipType.Pendant, _Equips[4]);
            _EquipTable.Add(EquipType.FaceAcc, _Equips[5]);
            _EquipTable.Add(EquipType.EyeAcc, _Equips[6]);
            _EquipTable.Add(EquipType.Earrings, _Equips[7]);
            _EquipTable.Add(EquipType.Weapon, _Equips[9]);
            _EquipTable.Add(EquipType.Top, _Equips[10]);
            _EquipTable.Add(EquipType.Shoulder, _Equips[11]);
            _EquipTable.Add(EquipType.SubWeapon, _Equips[12]);
            _EquipTable.Add(EquipType.Belt, _Equips[14]);
            _EquipTable.Add(EquipType.Bottom, _Equips[15]);
            _EquipTable.Add(EquipType.Gloves, _Equips[16]);
            _EquipTable.Add(EquipType.Cape, _Equips[17]);
            _EquipTable.Add(EquipType.Shoes, _Equips[19]);

            _TotalStats = new Stats();

            foreach (EquipSlot slot in _Equips)
                slot.OnUpdate += UpdateTotalStats;
        }

        public ItemEquip EquipItem (ItemEquip item)
        {
            if (item.EquipType == EquipType.Ring) return EquipRing(item);
            
            return _EquipTable[item.EquipType].ChangeItem(item);
        }

        public ItemEquip UnequipItem (int idx)
        {
            return _Equips[idx].Unequip();
        }

        public void UpdateTotalStats ()
        {
            List<Stats> tempList = new List<Stats>();
            foreach (EquipSlot slot in _Equips)
            {
                if (slot.IsEmpty) continue;
                tempList.Add(slot.Item.TotalStats);
            }

            _TotalStats = Stats.Sum(tempList.ToArray());
            OnUpdate?.Invoke(_TotalStats);
        }

#region Private Functions
        private ItemEquip EquipRing (ItemEquip item)
        {
            if (_Equips[18].IsEmpty) return _Equips[18].ChangeItem(item);
            if (_Equips[13].IsEmpty) return _Equips[13].ChangeItem(item);
            if (_Equips[8].IsEmpty) return _Equips[8].ChangeItem(item);
            if (_Equips[3].IsEmpty) return _Equips[3].ChangeItem(item);
            if (_Equips[0].IsEmpty) return _Equips[0].ChangeItem(item);

            return _Equips[18].ChangeItem(item);
        }

        private void UpdateTotalStats (ItemEquip item, bool isAdded)
        {
            if (item == null) return;

            
            if (isAdded)
                Stats.Add(_TotalStats, item.TotalStats);
            else
                Stats.Subtract(_TotalStats, item.TotalStats);
            
            OnUpdate?.Invoke(_TotalStats);
        }
    }
#endregion
#region EquipmenSlot Class
    [System.Serializable]
    public class EquipSlot
    {
        // Members
        [SerializeField] private EquipType _EquipType;
        [SerializeField] private bool _IsEmpty;
        [SerializeField] private ItemEquip _Item;


        public delegate void HandleUpdate(ItemEquip item, bool isAdded);
        public event HandleUpdate OnUpdate;

        public ItemEquip Item => _Item;
        public bool IsEmpty => _IsEmpty;
        public EquipType Type => _EquipType;

        public EquipSlot ()
        {
            _EquipType = EquipType.Any;
            _Item = null;
            _IsEmpty = true;
            OnUpdate?.Invoke(null, true);
        }

        public EquipSlot (EquipType type)
        {
            _EquipType = type;
            _Item = null;
            _IsEmpty = true;
            OnUpdate?.Invoke(null, true);
        }

        public ItemEquip ChangeItem (ItemEquip item)
        {
            if (item == null) return Unequip(); 

            if (item.EquipType != _EquipType) return item;

            ItemEquip returnItem = RemoveItem();
            AddItem(item);

            return returnItem;
        }

        public ItemEquip Unequip ()
        {
            ItemEquip returnItem = RemoveItem();
            return returnItem;
        }

        private ItemEquip RemoveItem ()
        {
            if (_IsEmpty) return null;

            ItemEquip returnItem = _Item;
            _Item = null;
            _IsEmpty = true;


            OnUpdate?.Invoke(returnItem, false);
            return returnItem;
        }

        private void AddItem (ItemEquip item)
        {
            _Item = item;
            _IsEmpty = false;
            OnUpdate?.Invoke(_Item, true);
        }

        public static void Swap (EquipSlot slot1, EquipSlot slot2)
        {
            if (slot1.Type != slot2.Type) return;

            ItemEquip item = slot2.RemoveItem();
            slot2.ChangeItem(slot1.RemoveItem());
            slot1.ChangeItem(item);
        }
    }
#endregion
}