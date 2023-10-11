using System;
using UnityEngine;
using ShopSystem;

namespace UISystem
{
    public class BuyButton : CustomButton
    {
        [Header("Buy Button Settings")]
        [SerializeField] private ShopItemUI _ShopItemUI;
        
        private ShopItem _ShopItem;

        public void Init (ShopItem shopItem)
        {
            _ShopItemUI.Init(shopItem);
            _ShopItem = shopItem;
        }

        protected override void ClickUpAction ()
        {
            _ShopItem.BuyLevel();
            _ShopItemUI.UpdateShopItem();
        }
    }
}