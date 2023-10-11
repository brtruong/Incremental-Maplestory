using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using CraftingSystem;
using InventorySystem;

namespace UISystem
{
    public class CraftingItemSlotButton : CustomButton
    {
        [SerializeField] private Image _Image;
        [SerializeField] private Sprite _NothingSprite;
        [SerializeField] private BaseItemEquip _Item;

        public void Init (BaseItemEquip item)
        {
            _Item = item;

            if (item == null) _Image.sprite = _NothingSprite;
            else _Image.sprite = item.Sprite;
        }

#region Inherited Functions
        protected override void ClickUpAction () => CraftingUI.Instance?.SelectItem(_Item);
        protected override void HoverAction (PointerEventData data)
        {
            if (_Item == null) return;
            ItemDisplayUI.Instance?.Open(_Item);
        }
        protected override void ExitAction (PointerEventData data) => ItemDisplayUI.Instance?.Close();
#endregion
    }
}