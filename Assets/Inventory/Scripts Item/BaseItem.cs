using UnityEngine;

namespace InventorySystem
{
    public enum ItemType
    {
        Equip, Use, Etc
    }

    public abstract class BaseItem : ScriptableObject
    {
        // Members
        [Header ("Base Item")]
        [SerializeField] protected int _ID;
        [SerializeField] protected ItemType _Type;
        [SerializeField] protected string _Name;
        [SerializeField] protected string _Description;
        [SerializeField] protected Sprite _Sprite;

        public int ID => _ID;
        public ItemType Type => _Type;
        public string Name => _Name;
        public string Description => _Description;
        public Sprite Sprite => _Sprite;

        public void ChangeID (int newID)
        {
            if ( newID <= 0) return;

            _ID = newID;
        }
    }
}
