using System.Collections.Generic;
using UnityEngine;
using UISystem;

using Newtonsoft.Json;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        // Members
        public static InventoryManager Instance {get; private set;}
        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        private const string SAVE_FILE = "InventoryData";

        [Header("Inventories In Game")]
        [SerializeField] private InventoryEquips[] _EquipInventories;
#region UnityFunctions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }
#endregion
#region Public Functions
        [ContextMenu("Save")]
        public void SaveInventories ()
        {
            _Logger.Log(gameObject, "Saving Invetories");
           
            SaveData save = new SaveData(){EquipInventories = _EquipInventories};
            SaveSystem.Save(save, GameSettings.SavePath, SAVE_FILE);
        }

        [ContextMenu("Load")]
        public void LoadInventories ()
        {
            _Logger.Log(gameObject, "Loading Inventory Data");

            SaveData save = (SaveData)SaveSystem.Load<SaveData>(GameSettings.SavePath, SAVE_FILE);
            _EquipInventories = save.EquipInventories;
            
            if (_EquipInventories == null) CreateNewInventories();
            else UpdateInventories();
        }

        public void AddEquip (ItemEquip item)
        {
            switch (item.EquipType)
            {
                case EquipType.Weapon:
                    _EquipInventories[0].AddItem(item);
                    break;
                case EquipType.SubWeapon:
                    _EquipInventories[1].AddItem(item);
                    break;
                case EquipType.Emblem:
                    _EquipInventories[2].AddItem(item);
                    break;
                case EquipType.Ring:
                    _EquipInventories[3].AddItem(item);
                    break;
                case EquipType.Pendant: 
                    _EquipInventories[4].AddItem(item);
                    break;
                case EquipType.FaceAcc:
                    _EquipInventories[5].AddItem(item);
                    break;
                case EquipType.EyeAcc:
                    _EquipInventories[6].AddItem(item);
                    break;
                case EquipType.Earrings:
                    _EquipInventories[7].AddItem(item);
                    break;
                case EquipType.Hat:
                    _EquipInventories[8].AddItem(item);
                    break;
                case EquipType.Top:
                    _EquipInventories[9].AddItem(item);
                    break;
                case EquipType.Bottom:
                    _EquipInventories[10].AddItem(item);
                    break;
                case EquipType.Gloves:
                    _EquipInventories[11].AddItem(item);
                    break;
                case EquipType.Shoes:
                    _EquipInventories[12].AddItem(item);
                    break;
                case EquipType.Cape:
                    _EquipInventories[13].AddItem(item);
                    break;
                case EquipType.Shoulder:
                    _EquipInventories[14].AddItem(item);
                    break;
                case EquipType.Belt:
                    _EquipInventories[15].AddItem(item);
                    break;
                default: break;
            }
        }
#endregion
#region Private Functions
        private void CreateNewInventories ()
        {
            _EquipInventories = new InventoryEquips[16];
            _EquipInventories[0] = new InventoryEquips(EquipType.Weapon);
            _EquipInventories[1] = new InventoryEquips(EquipType.SubWeapon);
            _EquipInventories[2] = new InventoryEquips(EquipType.Emblem);
            _EquipInventories[3] = new InventoryEquips(EquipType.Ring);
            _EquipInventories[4] = new InventoryEquips(EquipType.Pendant);
            _EquipInventories[5] = new InventoryEquips(EquipType.FaceAcc);
            _EquipInventories[6] = new InventoryEquips(EquipType.EyeAcc);
            _EquipInventories[7] = new InventoryEquips(EquipType.Earrings);
            _EquipInventories[8] = new InventoryEquips(EquipType.Hat);
            _EquipInventories[9] = new InventoryEquips(EquipType.Top);
            _EquipInventories[10] = new InventoryEquips(EquipType.Bottom);
            _EquipInventories[11] = new InventoryEquips(EquipType.Gloves);
            _EquipInventories[12] = new InventoryEquips(EquipType.Shoes);
            _EquipInventories[13] = new InventoryEquips(EquipType.Cape);
            _EquipInventories[14] = new InventoryEquips(EquipType.Shoulder);
            _EquipInventories[15] = new InventoryEquips(EquipType.Belt);

            InventoryUI.Instance?.GenerateUI(_EquipInventories);
        }

        private void UpdateInventories ()
        {
            foreach (InventoryEquips inventory in _EquipInventories)
                inventory.UpdateAllItems();

            InventoryUI.Instance?.GenerateUI(_EquipInventories);
        }

        [System.Serializable]
        private struct SaveData
        {
            public InventoryEquips[] EquipInventories;
        }
#endregion
    }
}