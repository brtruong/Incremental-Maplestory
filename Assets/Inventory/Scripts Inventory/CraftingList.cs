//using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Crafting_{}_List", menuName = "Inventory/Crafting List")]
    public class CraftingList : ScriptableObject
    {
        [field:SerializeField] public BaseItemEquip[] Items {get; private set;}
    }
}