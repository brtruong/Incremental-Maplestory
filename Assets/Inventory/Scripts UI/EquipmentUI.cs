using System.Collections.Generic;
using UnityEngine;

using CharacterSystem;
using InventorySystem;

namespace UISystem
{
    public class EquipmentUI : MonoBehaviour
    {
        // Memebers
        public static EquipmentUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _EquipmentWindow;
        [SerializeField] private Transform _EquipmentContainer;
        [SerializeField] private GameObject _EquipmentPrefab, _EquipmentSlotPrefab;
        [SerializeField] private GameObject _EmptySlot;

        [Header("Active Character's Equipment")]
        [SerializeField] private Character _ActiveCharacter;

        // Private
        private Dictionary<Character, GameObject> _WindowTable;

        public void Open ()
        {
            _EquipmentWindow.SetActive(true);
            SkillManagerUI.Instance?.Close();
        }
        public void Close () => _EquipmentWindow.SetActive(false);
        public void Toggle ()
        {
            if (_EquipmentWindow.activeSelf) Close();
            else Open();
        }

#region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);

            _WindowTable = new Dictionary<Character, GameObject>();
            _ActiveCharacter = null;
        }
#endregion
#region Public Functions
        public void GenerateEquipmentUI (Character character)
        {
            Equipment equipment = character.Equipment;
            UpdateEquipmentBase(equipment);

            GameObject newEquipmentWindow = Instantiate(_EquipmentPrefab, _EquipmentContainer);
            newEquipmentWindow.name = character.Base.Name + " - Equipment";

            for (int i = 0; i < Equipment.Size; i++)
            {   
                EquipmentSlotButton slotUI = Instantiate(_EquipmentSlotPrefab, newEquipmentWindow.transform).GetComponent<EquipmentSlotButton>();
                slotUI.Init(equipment[i]);
            }

            // Add Empty Game Objects to make Equipment UI display in the correct order
            Instantiate(_EmptySlot, newEquipmentWindow.transform).transform.SetSiblingIndex(1);
            Instantiate(_EmptySlot, newEquipmentWindow.transform).transform.SetSiblingIndex(3);
            Instantiate(_EmptySlot, newEquipmentWindow.transform).transform.SetSiblingIndex(21);
            
            newEquipmentWindow.SetActive(false);
            _WindowTable.Add(character, newEquipmentWindow);
        }

        public void RemoveAllWindows ()
        {
            foreach (GameObject o in _WindowTable.Values)
                Destroy(o);

            _WindowTable.Clear();
            _ActiveCharacter = null;
        }

        public void ChangeDisplayedEquipment (Character character)
        {
            if (character == null || _ActiveCharacter == character) return;

            // If Any - Deactivate Equipment Window of previous active character
            if (_ActiveCharacter != null) _WindowTable[_ActiveCharacter].SetActive(false);

            if (!_WindowTable.ContainsKey(character)) GenerateEquipmentUI(character);

            _WindowTable[character].SetActive(true);
            _ActiveCharacter = character;
        }

        public void EquipItem (EquipSlot equipSlot)
        {
            if (_ActiveCharacter == null || equipSlot == null || equipSlot.IsEmpty) return;

            ItemEquip returnItem = _ActiveCharacter.Equipment.EquipItem(equipSlot.Item);
            if (equipSlot.Item != returnItem) equipSlot.ChangeItem(returnItem);
        }
#endregion
#region Private Functions
        private void UpdateEquipmentBase (Equipment equipment)
        {
            if (DatabaseManager.Instance == null) return;

            Database database = DatabaseManager.Instance?.Database; 

            for (int i = 0; i < Equipment.Size; i++)
                if (!equipment[i].IsEmpty) equipment[i].Item.UpdateBase();
            
            equipment.UpdateTotalStats();
        }
#endregion
    }
}