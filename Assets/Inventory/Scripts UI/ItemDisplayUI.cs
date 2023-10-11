using System;
using UnityEngine;
using UnityEngine.UI;

using InventorySystem;

namespace UISystem
{
    public class ItemDisplayUI : MonoBehaviour
    {
        // Memebers
        public static ItemDisplayUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _ItemDisplayWindow;
        [SerializeField] private Text _TextItemName, _TextItemDescription;
        [SerializeField] private Image _ImageItem;

        private RectTransform _RectTransform;
        private Vector2 _Offset = new Vector2(0, -25);

#region Unity Functions
        private void Awake ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            _RectTransform = _ItemDisplayWindow.GetComponent<RectTransform>();
            Close();
        }
#endregion
#region Public Functions
        public void Open (Item item)
        {
            UpdateItem(item);
        }

        public void Open (BaseItemEquip item)
        {
            _TextItemName.text = item.Name;
            _ImageItem.sprite = item.Sprite;


            // output = (string , int)
            var output = GetStats(item);
            _TextItemDescription.text = output.Item1;

            // Update Text Transform to Fit
            Vector2 newSize = _TextItemDescription.rectTransform.sizeDelta;
            newSize.y = (_TextItemDescription.fontSize + 2) * output.Item2;
            _TextItemDescription.rectTransform.sizeDelta = newSize;

            UpdateWindowSize();

            _ItemDisplayWindow.SetActive(true);
            InvokeRepeating("UpdateWindowPosition", 0f, 0.02f);
        }

        public void Close ()
        {
            _ItemDisplayWindow.SetActive(false);
            CancelInvoke("updateWindowPosition");
        }

        public void SetWindowPosition (Vector2 newPosition)
        {
            _RectTransform.anchoredPosition = newPosition;
        }
#endregion
#region Private Functions
        private void UpdateItem (Item item)
        {
            if (item.IsEquip)
            {
                _TextItemName.text = item.Name;
                _ImageItem.sprite = item.Base.Sprite;


                // output = (string , int)
                var output = GetStats((ItemEquip)item);
                _TextItemDescription.text = output.Item1;

                // Update Text Transform to Fit
                Vector2 newSize = _TextItemDescription.rectTransform.sizeDelta;
                newSize.y = (_TextItemDescription.fontSize + 2) * output.Item2;
                _TextItemDescription.rectTransform.sizeDelta = newSize;

                UpdateWindowSize();

                _ItemDisplayWindow.SetActive(true);
                InvokeRepeating("UpdateWindowPosition", 0f, 0.02f);
            }
        }

        private void UpdateWindowPosition ()
        {
            _RectTransform.anchoredPosition = Input.mousePosition;
            _RectTransform.anchoredPosition += _Offset;
        }

        private void UpdateWindowSize ()
        {
            Vector2 newSize = _RectTransform.sizeDelta;
            newSize.y = 240 + _TextItemDescription.rectTransform.sizeDelta.y;
            _RectTransform.sizeDelta = newSize;
        }

        private (string, int) GetStats (ItemEquip item)
        {
            int numLines = 1;
            string str = string.Concat("Type: ", item.EquipType.ToString().ToUpper(), "\n");
            
            if (item.TotalStats.STR != 0)
            {
                str += "STR: +" + item.TotalStats.STR;

                if (item.FlameStats.STR != 0)
                    str += " (" + item.BaseStats.STR + " + " + item.FlameStats.STR +")";

                str += "\n";
                numLines++;
            }
            
            if (item.TotalStats.DEX != 0)
            {
                str += "DEX: +" + item.TotalStats.DEX;

                if (item.FlameStats.DEX != 0)
                    str += " (" + item.BaseStats.DEX + " + " + item.FlameStats.DEX +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.INT != 0)
            {
                str += "INT: +" + item.TotalStats.INT;

                if (item.FlameStats.INT != 0)
                    str += " (" + item.BaseStats.INT + " + " + item.FlameStats.INT +")";

                str += "\n";
                numLines++;
            }
            
            if (item.TotalStats.LUK != 0)
            {
                str += "LUK: +" + item.TotalStats.LUK;

                if (item.FlameStats.LUK != 0)
                    str += " (" + item.BaseStats.LUK + " + " + item.FlameStats.LUK +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.MaxHP != 0)
            {
                str += "MaxHP: +" + item.TotalStats.MaxHP;

                if (item.FlameStats.MaxHP != 0)
                    str += " (" + item.BaseStats.MaxHP + " + " + item.FlameStats.MaxHP +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.MaxMP != 0)
            {
                str += "MaxMP: +" + item.TotalStats.MaxMP;

                if (item.FlameStats.MaxMP != 0)
                    str += " (" + item.BaseStats.MaxMP + " + " + item.FlameStats.MaxMP +")";

                str += "\n";
                
                numLines++;
            }
            if (item.TotalStats.ATT != 0)
            {
                str += "Attack Power: +" + item.TotalStats.ATT;

                if (item.FlameStats.ATT != 0)
                    str += " (" + item.BaseStats.ATT + " + " + item.FlameStats.ATT +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.MATT != 0)
            {
                str += "Magic Attack: +" + item.TotalStats.MATT;

                if (item.FlameStats.MATT != 0)
                    str += " (" + item.BaseStats.MATT + " + " + item.FlameStats.MATT +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.DEF != 0)
            {
                str += "DEF: +" + item.TotalStats.DEF;

                if (item.FlameStats.DEF != 0)
                    str += " (" + item.BaseStats.DEF + " + " + item.FlameStats.DEF +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctAllStat != 0)
            {
                str += "All Stats: +" + item.TotalStats.PctAllStat;

                if (item.FlameStats.PctAllStat != 0)
                    str += " (" + item.BaseStats.PctAllStat + " + " + item.FlameStats.PctAllStat +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctBossDMG != 0)
            {
                str += "Boss Damage: +" + item.TotalStats.PctBossDMG;

                if (item.FlameStats.PctBossDMG != 0)
                    str += " (" + item.BaseStats.PctBossDMG + " + " + item.FlameStats.PctBossDMG +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctIED != 0)
            {
                str += "Ignore Enemy DEF: +" + item.TotalStats.PctIED;

                if (item.FlameStats.PctIED != 0)
                    str += " (" + item.BaseStats.PctIED + " + " + item.FlameStats.PctIED +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctDMG != 0)
            {
                str += "Damage: +" + item.TotalStats.PctDMG;

                if (item.FlameStats.PctDMG != 0)
                    str += " (" + item.BaseStats.PctDMG + " + " + item.FlameStats.PctDMG +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctCrt != 0)
            {
                str += "Critical Rate: +" + item.TotalStats.PctCrt;

                if (item.FlameStats.PctCrt != 0)
                    str += " (" + item.BaseStats.PctCrt + " + " + item.FlameStats.PctCrt +")";

                str += "\n";
                numLines++;
            }

            if (item.TotalStats.PctCrtDMG != 0)
            {
                str += "Critical Damage: +" + item.TotalStats.PctCrtDMG;

                if (item.FlameStats.PctCrtDMG != 0)
                    str += " (" + item.BaseStats.PctCrtDMG + " + " + item.FlameStats.PctCrtDMG +")";

                str += "\n";
                numLines++;
            }

            return (str, numLines);
        }
        
        private (string, int) GetStats (BaseItemEquip item)
        {
            int numLines = 1;
            string str = string.Concat("Type: ", item.EquipType.ToString().ToUpper(), "\n");
            
            if (item.Stats.STR != 0)
            {
                str += "STR: +" + item.Stats.STR + "\n";
                numLines++;
            }
            
            if (item.Stats.DEX != 0)
            {
                str += "DEX: +" + item.Stats.DEX + "\n";
                numLines++;
            }

            if (item.Stats.INT != 0)
            {
                str += "INT: +" + item.Stats.INT + "\n";
                numLines++;
            }
            
            if (item.Stats.LUK != 0)
            {
                str += "LUK: +" + item.Stats.LUK + "\n";
                numLines++;
            }

            if (item.Stats.MaxHP != 0)
            {
                str += "MaxHP: +" + item.Stats.MaxHP + "\n";
                numLines++;
            }

            if (item.Stats.MaxMP != 0)
            {
                str += "MaxMP: +" + item.Stats.MaxMP + "\n";
                
                numLines++;
            }
            if (item.Stats.ATT != 0)
            {
                str += "Attack Power: +" + item.Stats.ATT + "\n";
                numLines++;
            }

            if (item.Stats.MATT != 0)
            {
                str += "Magic Attack: +" + item.Stats.MATT + "\n";
                numLines++;
            }

            if (item.Stats.DEF != 0)
            {
                str += "DEF: +" + item.Stats.DEF + "\n";
                numLines++;
            }

            if (item.Stats.PctAllStat != 0)
            {
                str += "All Stats: +" + item.Stats.PctAllStat + "%\n";
                numLines++;
            }

            if (item.Stats.PctBossDMG != 0)
            {
                str += "Boss Damage: +" + item.Stats.PctBossDMG + "%\n";
                numLines++;
            }

            if (item.Stats.PctIED != 0)
            {
                str += "Ignore Enemy DEF: +" + item.Stats.PctIED + "%\n";
                numLines++;
            }

            if (item.Stats.PctDMG != 0)
            {
                str += "Damage: +" + item.Stats.PctDMG + "%\n";
                numLines++;
            }

            if (item.Stats.PctCrt != 0)
            {
                str += "Critical Rate: +" + item.Stats.PctCrt + "%\n";
                numLines++;
            }

            if (item.Stats.PctCrtDMG != 0)
            {
                str += "Critical Damage: +" + item.Stats.PctCrtDMG + "%\n";
                numLines++;
            }

            return (str, numLines);
        }
#endregion
    }
}