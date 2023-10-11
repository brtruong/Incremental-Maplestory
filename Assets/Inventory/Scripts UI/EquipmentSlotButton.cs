using UnityEngine;
using UnityEngine.EventSystems;

using InventorySystem;

namespace UISystem
{
    public class EquipmentSlotButton : CustomButton
    {
        [SerializeField] private InventorySlotUI _InventorySlotUI;
        [SerializeField] private EquipSlot _EquipSlot;

        private const float DOUBLE_CLICK_TIME = 0.5f;
        private float _TimeStart;

        public void Init (EquipSlot equipSlot)
        {
            _EquipSlot = equipSlot;
            _InventorySlotUI.Init(equipSlot, false);
        }
        
        public void Equip (EquipSlot equipSlot)
        {
            if (equipSlot.IsEmpty) return;

            ItemEquip returnItem = _EquipSlot.ChangeItem(equipSlot.Item);
            equipSlot.ChangeItem(returnItem);
        }
#region Inherited Functions
        protected override void HoverAction (PointerEventData data)
        {
            if (_EquipSlot.IsEmpty) return;
            ItemDisplayUI.Instance?.Open(_EquipSlot.Item);
        }
        protected override void DragStartAction (PointerEventData data)
        {
            if (_EquipSlot.IsEmpty) return;
            CursorUI.Instance.StartDrag(_EquipSlot.Item.Base.Sprite);
            _TimeStart = Time.time;
        }
        protected override void DragEndAction (PointerEventData data) 
        {
            foreach (GameObject g in data.hovered)
            {
                if (g == gameObject)
                {
                    if (Time.time < _TimeStart + DOUBLE_CLICK_TIME)
                        InventoryManager.Instance?.AddEquip(_EquipSlot.Unequip());
                    return;
                }

                if (g.tag == "Inventory Slot")
                {
                    g.GetComponent<InventorySlotButton>().SwapEquips(_EquipSlot);
                    return;
                }
            }
        }
        protected override void ExitAction (PointerEventData data) => ItemDisplayUI.Instance?.Close();
#endregion
    }
}