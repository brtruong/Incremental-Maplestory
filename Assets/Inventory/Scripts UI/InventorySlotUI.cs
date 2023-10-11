using System;
using UnityEngine;
using UnityEngine.UI;
using InventorySystem;

namespace UISystem
{
    public class InventorySlotUI : MonoBehaviour
    {
        // Members
        [Header("Settings")]
        [SerializeField] private Image _ImageItem;
        [SerializeField] private Text _Text;
        [SerializeField] private EquipSlot _EquipSlot;

        public void Init (EquipSlot equipSlot, bool isInventory)
        {
            _EquipSlot = equipSlot;
            _EquipSlot.OnUpdate += UpdateEquipSlot;
            if (!isInventory) 
                _Text.text =  EquipType.GetName(typeof(EquipType), equipSlot.Type).ToUpper();
            
            UpdateEquipSlot(null, false);
        }

#region Private Functions
        private void UpdateEquipSlot (Item item, bool isAdded)
        {
            _ImageItem.enabled = true;
            if (_EquipSlot.IsEmpty) _ImageItem.enabled = false;
            else _ImageItem.sprite = _EquipSlot.Item.Base.Sprite;
        }

        private void OnEnable ()
        {
            if (_EquipSlot != null) _EquipSlot.OnUpdate += UpdateEquipSlot;
        }
        
        private void OnDisable ()
        {
            if (_EquipSlot != null) _EquipSlot.OnUpdate -= UpdateEquipSlot;
        }
#endregion
    }
}