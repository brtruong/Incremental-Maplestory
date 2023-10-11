using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class ItemUse : Item
    {
        // Members
        [SerializeField] private int _ItemCount;
        private BaseItemUse _BaseItemUse;

        public override bool IsEquip => false;
        public override bool IsUse => true;
        public override bool IsEtc => false;
        public int Count => _ItemCount; 

        private const int MAX_COUNT = 9999;

        public ItemUse (BaseItemUse baseItemUse)
        {
            _ID = baseItemUse.ID;
            _BaseItemUse = baseItemUse;
            _ItemCount = 1;
        }

        public ItemUse (BaseItemUse baseItemUse, int count)
        {
            _ID = baseItemUse.ID;
            _BaseItemUse = baseItemUse;
            _ItemCount = count;
        }

    }
}