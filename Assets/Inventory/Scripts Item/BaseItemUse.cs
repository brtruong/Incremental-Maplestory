using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Use_", menuName = "Items/Use/Default")]
    [System.Serializable]
    public class BaseItemUse : BaseItem
    {
        private void Awake ()
        {
            this._Type = ItemType.Use;
        }

        public virtual void Use ()
        {
            Debug.Log("Using Use Item");
        }
    }
}