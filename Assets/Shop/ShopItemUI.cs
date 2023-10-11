using UnityEngine;
using UnityEngine.UI;
using MesoSystem;
using ShopSystem;

namespace UISystem
{    
    public class ShopItemUI : MonoBehaviour
    {
        // Members
        [Header("Settings")]
        [SerializeField] private Image _ImageIcon;
        [SerializeField] private Text _TextName, _TextCost;
        
        [Header("Shop Item")]
        [SerializeField] private ShopItem _ShopItem;

#region Public Functions
        public void Init (ShopItem shopItem)
        {
            _ShopItem = shopItem;
            transform.name = _ShopItem.Name;
            UpdateShopItem();
        }

        public void UpdateShopItem ()
        {
            _ImageIcon.sprite = _ShopItem.Icon;
            _TextName.text = _ShopItem.Name + " - Level: " + _ShopItem.Level;
            _TextCost.text = _ShopItem.Cost.ToString();
        }
#endregion
    }
}