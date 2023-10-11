using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Use_", menuName = "Items/Use/Potion")]
    [System.Serializable]
    public class PotionItem : BaseItemUse
    {
        [field:Header("Potion Base")]
        [field:SerializeField] public bool HealHP {get; private set;}
        [field:SerializeField] public bool HealMP {get; private set;}
        [field:SerializeField] public bool HealPercentage {get; private set;}
        [field:SerializeField] public int HealValue {get; private set;}
        
        private void Awake ()
        {
            this._Type = ItemType.Use;
        }

        public override void Use ()
        {
            Debug.Log("Using Potion");
        }
    }
}