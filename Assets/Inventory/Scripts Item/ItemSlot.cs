using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{   
    [System.Serializable]
    public class ItemSlot
    {
        // Members
        [SerializeField] private Item _Item;
        [SerializeField] private int _MaxQuantity, _Quantity;
        [SerializeField] private bool _IsEmpty, _IsFull;
        private static int _DefaultMaxQuantity = 256;

        public delegate void HandleUpdate();
        public event HandleUpdate OnUpdate;
        
        public Item Item => _Item;
        public int MaxQuantity => _MaxQuantity;
        public int Quantity => _Quantity;
        public bool IsEmpty => _IsEmpty;
        public bool IsFull => _IsFull;

#region Constructor
        public ItemSlot ()
        {
            ClearSlot();
        }

        public ItemSlot (Item item)
        {
            SetItem(item);
        }
#endregion
#region Public Functions
        public void SetItem (Item item)
        {
            ClearSlot();
        
            if (item == null)
            {
                OnUpdate?.Invoke();
                return;
            }
            _Item = item;
            _IsEmpty = false;
            _Quantity = 1;

            if (item.IsEquip)
            {
                _MaxQuantity = 1;
                _IsFull = true;
            }

            OnUpdate?.Invoke();
        }

        public int SetItem (Item item, int num)
        {
            if (num < 1) return num;

            ClearSlot();
            if (item == null)
            {
                OnUpdate?.Invoke();
                return num;
            }
            
            _Item = item;
            _IsEmpty = false;

            if (item.IsEquip)
            {
                _MaxQuantity = 1;
                _Quantity = 1;
                _IsFull = true;
                OnUpdate?.Invoke();
                return 0;
            }
            else 
            {
                OnUpdate?.Invoke();
                return AddQuantity(num);
            }
        }

        public int AddQuantity (int num)
        {
            if (_IsFull || num < 1) return num;

            _IsEmpty = false;
            _Quantity += num;
            int overFlow = _Quantity - _MaxQuantity;
            if (overFlow < 0)
            {
                OnUpdate?.Invoke();
                return 0;
            }
            
            _IsFull = true;
            _Quantity = _MaxQuantity;
            OnUpdate?.Invoke();
            return overFlow;
        }

        public void ClearSlot ()
        {
            _Item = null;
            _Quantity = 0;
            _IsEmpty = true;
            _IsFull = false;
            _MaxQuantity = _DefaultMaxQuantity;
            OnUpdate?.Invoke();
        }

        private void CopySlot (ItemSlot newItemSlot)
        {
            _Item = newItemSlot.Item;
            _MaxQuantity = newItemSlot.MaxQuantity;
            _Quantity = newItemSlot.Quantity;
            _IsEmpty = newItemSlot.IsEmpty;
            _IsFull = newItemSlot.IsFull;
            OnUpdate?.Invoke();
        }
#endregion
#region Static Functions
        public static void Swap (ItemSlot slot1, ItemSlot slot2)
        {
            ItemSlot temp = new ItemSlot();
            temp.CopySlot(slot1);
            slot1.CopySlot(slot2);
            slot2.CopySlot(temp);
        }
#endregion
    }
}