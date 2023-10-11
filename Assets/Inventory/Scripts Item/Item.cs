using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        // Members
        [SerializeField] protected int _ID;
        [SerializeField] protected BaseItem _BaseItem;
    
        public int ID => _ID;
        public BaseItem Base => _BaseItem;
        public string Name => _BaseItem.Name;

        public virtual bool IsEquip => false;
        public virtual bool IsUse => false;
        public virtual bool IsEtc => false;

        public virtual void UpdateBase () => _BaseItem = DatabaseManager.GetBaseItem(_ID);
        public virtual object CaptureSave (){return new SaveData{ID = _ID};}

        private struct SaveData
        {
            public int ID;
        }
    }
}