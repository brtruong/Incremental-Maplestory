using UnityEngine;
using UnityEngine.UI;

using InventorySystem;
using UISystem;

namespace CraftingSystem
{
    public class CraftingUI : MonoBehaviour
    {
        public static CraftingUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _CraftingWindow;
        [SerializeField] private Transform _CraftingContainer;
        [SerializeField] private GameObject _PrefabCraftingInventory, _PrefabItemSlotButton;
        [SerializeField] private CraftingList _CraftingList;
        [SerializeField] private Image _SelectedItemImage, _MaterialItemImage;

        private BaseItemEquip _SelectedItem;

#region Unity Functions
        private void Awake ()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            GenerateCraftingUI();
        }
#endregion
#region Public Functions
        public void Open ()
        {
            _CraftingWindow.SetActive(true);
            WindowSelector.Select(_CraftingWindow.transform);
        }
        public void Close () => _CraftingWindow.SetActive(false);
        public void Toggle ()
        {
            if (_CraftingWindow.activeSelf) Close();
            else Open();
        }

        public void CraftSelectedItem ()
        {
            // Not Done. Needs to use crafting materials
            InventoryManager.Instance?.AddEquip(new ItemEquip(_SelectedItem));
        }

        public void SelectItem (BaseItemEquip item)
        {
            if (item == null)
            {
                _SelectedItemImage.enabled = false;
                _MaterialItemImage.enabled = false;
                return;
            }

            _SelectedItemImage.enabled = true;
            _MaterialItemImage.enabled = true;

            _SelectedItem = item;
            _SelectedItemImage.sprite = item.Sprite;
            BaseItemEtc materialItem = item.MaterialItem;
            if (materialItem != null) _MaterialItemImage.sprite = materialItem.Sprite;
            else _MaterialItemImage.enabled = false;
            
        }
#endregion
#region Private Functions
        private void GenerateCraftingUI ()
        {
            Transform craftingInv = Instantiate(_PrefabCraftingInventory, _CraftingContainer).transform;

            // For Each Crafting slot 
            foreach (BaseItemEquip item in _CraftingList.Items)
                Instantiate(_PrefabItemSlotButton, craftingInv).GetComponent<CraftingItemSlotButton>().Init(item);
        }
#endregion
    }
}