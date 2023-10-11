using UnityEngine;
using UnityEngine.EventSystems;

using InventorySystem;

namespace UISystem
{
    public class InventorySlotButton : CustomButton
    {
        [SerializeField] private InventorySlotUI _InventorySlotUI;
        private EquipSlot _EquipSlot;

        private delegate bool BoolDelegate();
        private BoolDelegate _CheckEmpty;

        private delegate void VoidDelegate();
        private VoidDelegate _DisplayItem, _StartDrag, _DoubleClick;

        private const float DOUBLE_CLICK_TIME = 0.5f;
        private float _TimeStart;

        public void Init (EquipSlot equipSlot)
        {
            _EquipSlot = equipSlot;
            _InventorySlotUI.Init(equipSlot, true);

            _CheckEmpty = delegate {return _EquipSlot.IsEmpty;};
            _StartDrag = delegate {CursorUI.Instance.StartDrag(_EquipSlot.Item.Base.Sprite);};
            _DisplayItem = delegate {if (!_EquipSlot.IsEmpty) ItemDisplayUI.Instance?.Open(_EquipSlot.Item);};
            _DoubleClick = delegate {EquipmentUI.Instance?.EquipItem(_EquipSlot);};
        }

        public void SwapEquips (EquipSlot equipSlot)
        {
            if (_EquipSlot == null) return;

            EquipSlot.Swap(_EquipSlot, equipSlot);
        }
#region Inherited Functions
        protected override void DragStartAction (PointerEventData data)
        {
            if (_CheckEmpty()) return;

            _StartDrag();
            ExitAction(data);
            _TimeStart = Time.time;
        }
        protected override void DragEndAction (PointerEventData data) 
        {
            foreach (GameObject g in data.hovered)
            {
                if (g == gameObject)
                {
                    if (Time.time < _TimeStart + DOUBLE_CLICK_TIME) _DoubleClick();
                    return;
                }

                if (g.tag == "Inventory Slot")
                {
                    if (_EquipSlot != null) g.GetComponent<InventorySlotButton>().SwapEquips(_EquipSlot);
                    return;
                }

                if (g.tag == "Equipment Slot")
                {
                    if (_EquipSlot != null) g.GetComponent<EquipmentSlotButton>().Equip(_EquipSlot);
                    return;
                }
            }
        }
        protected override void HoverAction (PointerEventData data) => _DisplayItem();
        protected override void ExitAction (PointerEventData data) => ItemDisplayUI.Instance?.Close();
#endregion
    }
}