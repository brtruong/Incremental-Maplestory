using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Etc_", menuName = "Items/Etc")]
    [System.Serializable]
    public class BaseItemEtc : BaseItem
    {
        private void Awake ()
        {
            this._Type = ItemType.Etc;
        }
    }
}