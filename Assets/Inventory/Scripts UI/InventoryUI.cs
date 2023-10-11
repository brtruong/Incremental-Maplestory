using UnityEngine;
using UnityEngine.UI;
using InventorySystem;

namespace UISystem
{
    public class InventoryUI : MonoBehaviour
    {
        // Members
        public static InventoryUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _InventoryWindow;
        [SerializeField] private Transform _InventoryContainer;
        [SerializeField] private GameObject _InvetoryPrefab, _ItemSlotPrefab;

        private GameObject[] _InventoryWindows;
        private int _ActiveTab, _ActiveEquipTab;

        public void Open ()
        {
            _InventoryWindow.SetActive(true);
            WindowSelector.Select(_InventoryWindow.transform);
        }
        public void Close () => _InventoryWindow.SetActive(false);
        public void Toggle () => _InventoryWindow.SetActive(!_InventoryWindow.activeSelf);

#region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }
#endregion
#region Public Functions
        public void GenerateUI (InventoryEquips[] inventories)
        {
            _InventoryWindows = new GameObject[inventories.Length];
            for (int i=0; i < inventories.Length; i++)
            {
                _InventoryWindows[i] = Instantiate(_InvetoryPrefab, _InventoryContainer);
                _InventoryWindows[i].name = string.Concat("Inventory - ", inventories[i].Type.ToString());
                foreach (EquipSlot equipSlot in inventories[i].EquipSlots)
                    Instantiate(_ItemSlotPrefab, _InventoryWindows[i].transform).GetComponent<InventorySlotButton>().Init(equipSlot);
            
                _InventoryWindows[i].SetActive(false);
            }
            _InventoryWindows[0].SetActive(true);
        }

        public void RemoveAllWindows ()
        {
            foreach (GameObject o in _InventoryWindows)
                Destroy(o);
        }

        public void SwitchTabs (int tabNum)
        {
            _InventoryWindows[_ActiveTab].SetActive(false);

            if (tabNum == -1) _ActiveTab = _ActiveEquipTab;
            else if (tabNum < _InventoryWindows.Length) _ActiveTab = _ActiveEquipTab = tabNum;

            _InventoryWindows[_ActiveTab].SetActive(true);
        }
#endregion
    }
}