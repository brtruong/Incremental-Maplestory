using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public enum EquipType
    {
        Any, Weapon, SubWeapon, Emblem, Ring,
        Pendant, FaceAcc, EyeAcc, Earrings,
        Hat, Top, Bottom, Gloves, Shoes, Cape, Shoulder, Belt 
    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "newItem", menuName = "Items/EquipItem")]
    public class BaseItemEquip : BaseItem
    {
        // Members
        [Header ("Equip Item")]
        [SerializeField] protected int m_LevelRequirement;
        [SerializeField] protected EquipType _EquipType;
        
        [Header ("Stats")]
        [SerializeField] protected Stats m_BaseStats;
        protected Stats m_BonusStats, m_TotalStats;

        [Header ("Crafting Requirements")]
        [SerializeField] protected BaseItemEtc materialItem;
        [SerializeField] protected int materialCount;

        public EquipType EquipType => _EquipType;
        public BaseItemEtc MaterialItem => materialItem;
        public int MaterialCount => materialCount;
        public Stats Stats => m_BaseStats;

#region Unity Functions
        private void Awake ()
        {
            this._Type = ItemType.Equip;
            m_BonusStats = new Stats();
            m_TotalStats = m_BaseStats;
        }
#endregion
#region Public Functions
        public void UpdateStats ()
        {
            m_TotalStats = Stats.Sum(new Stats[]{m_BaseStats, m_BonusStats});
        }

        public (string, int) PrintString ()
        {
            UpdateStats();

            string str = "";
            int numLines = 0;
            
            if (m_TotalStats.STR != 0)
            {
                str += "STR: +" + m_TotalStats.STR + "\n";
                numLines++;
            }
            
            if (m_TotalStats.DEX != 0)
            {
                str += "DEX: +" + m_TotalStats.DEX + "\n";
                numLines++;
            }

            if (m_TotalStats.INT != 0)
            {
                str += "INT: +" + m_TotalStats.INT + "\n";
                numLines++;
            }
            
            if (m_TotalStats.LUK != 0)
            {
                str += "LUK: +" + m_TotalStats.LUK + "\n";
                numLines++;
            }

            if (m_TotalStats.MaxHP != 0)
            {
                str += "MaxHP: +" + m_TotalStats.MaxHP + "\n";
                numLines++;
            }

            if (m_TotalStats.MaxMP != 0)
            {
                str += "MaxMP: +" + m_TotalStats.MaxMP + "\n";
                
                numLines++;
            }
            if (m_TotalStats.ATT != 0)
            {
                str += "Attack Power: +" + m_TotalStats.ATT + "\n";
                numLines++;
            }

            if (m_TotalStats.MATT != 0)
            {
                str += "Magic Attack: +" + m_TotalStats.MATT + "\n";
                numLines++;
            }

            if (m_TotalStats.DEF != 0)
            {
                str += "DEF: +" + m_TotalStats.DEF + "\n";
                numLines++;
            }

            if (m_TotalStats.PctAllStat != 0)
            {
                str += "All Stats: +" + m_TotalStats.PctAllStat + "%\n";
                numLines++;
            }

            if (m_TotalStats.PctBossDMG != 0)
            {
                str += "Boss Damage: +" + m_TotalStats.PctBossDMG + "%\n";
                numLines++;
            }

            if (m_TotalStats.PctIED != 0)
            {
                str += "Ignore Enemy DEF: +" + m_TotalStats.PctIED + "%\n";
                numLines++;
            }

            if (m_TotalStats.PctDMG != 0)
            {
                str += "Damage: +" + m_TotalStats.PctDMG + "%\n";
                numLines++;
            }

            if (m_TotalStats.PctCrt != 0)
            {
                str += "Critical Rate: +" + m_TotalStats.PctCrt + "%\n";
                numLines++;
            }

            if (m_TotalStats.PctCrtDMG != 0)
            {
                str += "Critical Damage: +" + m_TotalStats.PctCrtDMG + "%\n";
                numLines++;
            }

            return (str, numLines);
        }
#endregion
    }
}