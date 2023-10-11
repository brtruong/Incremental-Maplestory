using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class ItemEquip : Item
    {
        // Members
        [SerializeField] private Stats _BaseStats, _FlameStats, _TotalStats;
        private BaseItemEquip _BaseItemEquip;

        public override bool IsEquip => true;
        public override bool IsUse => false;
        public override bool IsEtc => false;

        public EquipType EquipType => _BaseItemEquip.EquipType;
        public Stats BaseStats => _BaseStats;
        public Stats FlameStats => _FlameStats;
        public Stats TotalStats => _TotalStats;

        public ItemEquip () => _ID = -1;

        public ItemEquip (BaseItemEquip baseItemEquip)
        {
            _ID = baseItemEquip.ID;
            _BaseItem = baseItemEquip;
            _BaseItemEquip = baseItemEquip;
            _BaseStats = baseItemEquip.Stats;

            RandomizeFlameStats();
            UpdateStats();
        }

        public void RandomizeFlameStats ()
        {
            _FlameStats = new Stats();
            _FlameStats.STR += Random.Range(0, 11);
            _FlameStats.DEX += Random.Range(0, 11);
            _FlameStats.INT += Random.Range(0, 11);
            _FlameStats.LUK += Random.Range(0, 11);
            UpdateStats();
        }

        public override void UpdateBase ()
        {
            base.UpdateBase();
            _BaseItemEquip = (BaseItemEquip)_BaseItem;
        }

        private void UpdateStats () => _TotalStats = Stats.Sum(new Stats[]{_BaseStats, _FlameStats});

        public override object CaptureSave ()
        {
            ItemSaveData save = new ItemSaveData(){
                ID = _ID,
                BaseStats = _BaseStats,
                FlameStats = _FlameStats,
                TotalStats = _TotalStats
            };
            return save;
        }

        public struct ItemSaveData
        {
            public int ID;
            public Stats BaseStats, FlameStats, TotalStats;
        }
    }
}