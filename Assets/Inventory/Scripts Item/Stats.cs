using UnityEngine;
public enum StatType
{
    STR, DEX, INT, LUK
}

namespace InventorySystem
{

    [System.Serializable]
    public class Stats
    {
        public int STR;
        public int DEX;
        public int INT;
        public int LUK;
        public int MaxHP;
        public int MaxMP;
        public int ATT;
        public int MATT;
        public int DEF;
        public int PctAllStat;
        public int PctBossDMG;
        public int PctIED;
        public int PctDMG;
        public int PctCrt;
        public int PctCrtDMG;
        public int PctMastery;

        public static Stats Sum (Stats[] StatsList)
        {
            Stats newStats = new Stats();
            
            foreach (Stats c in StatsList)
            {
                newStats.STR += c.STR;
                newStats.DEX += c.DEX;
                newStats.INT += c.INT;
                newStats.LUK += c.LUK;
                newStats.MaxHP += c.MaxHP;
                newStats.MaxMP += c.MaxMP;
                newStats.ATT += c.ATT;
                newStats.MATT += c.MATT;
                newStats.DEF += c.DEF;
                newStats.PctAllStat += c.PctAllStat;
                newStats.PctBossDMG += c.PctBossDMG;
                newStats.PctIED += c.PctIED;
                newStats.PctDMG += c.PctDMG;
                newStats.PctCrt += c.PctCrt;
                newStats.PctCrtDMG += c.PctCrtDMG;
                newStats.PctMastery += c.PctMastery;
            }

            return newStats;
        }

        public static void Add (Stats stats1, Stats stats2)
        {
            stats1.STR += stats2.STR;
            stats1.DEX += stats2.DEX;
            stats1.INT += stats2.INT;
            stats1.LUK += stats2.LUK;
            stats1.MaxHP += stats2.MaxHP;
            stats1.MaxMP += stats2.MaxMP;
            stats1.ATT += stats2.ATT;
            stats1.MATT += stats2.MATT;
            stats1.DEF += stats2.DEF;
            stats1.PctAllStat += stats2.PctAllStat;
            stats1.PctBossDMG += stats2.PctBossDMG;
            stats1.PctIED += stats2.PctIED;
            stats1.PctDMG += stats2.PctDMG;
            stats1.PctCrt += stats2.PctCrt;
            stats1.PctCrtDMG += stats2.PctCrtDMG;
            stats1.PctMastery += stats2.PctMastery;
        } 

        public static void Subtract (Stats stats1, Stats stats2)
        {
            stats1.STR -= stats2.STR;
            stats1.DEX -= stats2.DEX;
            stats1.INT -= stats2.INT;
            stats1.LUK -= stats2.LUK;
            stats1.MaxHP -= stats2.MaxHP;
            stats1.MaxMP -= stats2.MaxMP;
            stats1.ATT -= stats2.ATT;
            stats1.MATT -= stats2.MATT;
            stats1.DEF -= stats2.DEF;
            stats1.PctAllStat -= stats2.PctAllStat;
            stats1.PctBossDMG -= stats2.PctBossDMG;
            stats1.PctIED -= stats2.PctIED;
            stats1.PctDMG -= stats2.PctDMG;
            stats1.PctCrt -= stats2.PctCrt;
            stats1.PctCrtDMG -= stats2.PctCrtDMG;
            stats1.PctMastery -= stats2.PctMastery;
        } 

        public static Stats Randomize ()
        {
            Stats newStats = new Stats();
            newStats.STR += 10;
            newStats.DEX += 5;
            newStats.INT += 5;
            newStats.LUK += 10;

            return newStats;
        }

        public Stats ()
        {
            STR = DEX = INT = LUK = 0;
            MaxHP = MaxMP = DEF = 0;
            ATT = MATT = 0;
            PctAllStat = 0;
            PctBossDMG = PctIED = PctDMG = 0;
            PctCrt = PctCrtDMG = 0;
            PctMastery = 0;
        }

        public int GetStat (StatType type)
        {
            if (type == StatType.STR) return STR;
            if (type == StatType.DEX) return DEX;
            if (type == StatType.INT) return INT;
            if (type == StatType.LUK) return LUK;
            
            return -1;
        }
    }
}