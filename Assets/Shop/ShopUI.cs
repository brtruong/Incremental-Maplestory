using System.Collections.Generic;
using UnityEngine;

using CharacterSystem;
using UISystem;

namespace ShopSystem
{
    public class ShopUI : MonoBehaviour
    {
        // Members
        public static ShopUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _ShopWindow;
        [SerializeField] private Transform _ShopItemContainer;
        [SerializeField] private GameObject _ShopItemPrefab;

        [SerializeField] private List<ShopItem> _ShopItems;

        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);

            _ShopItems = new List<ShopItem>();
        }
#region Public Functions
        public void Open ()
        {
            _ShopWindow.SetActive(true);
            WindowSelector.Select(_ShopWindow.transform);
        }
        public void Close () => _ShopWindow.SetActive(false);
        public void Toggle ()
        {
            if (_ShopWindow.activeSelf) Close();
            else Open();
        }

        public void Clear ()
        {
            foreach (Transform t in _ShopItemContainer)
                Destroy(t.gameObject);
        }
        public void AddCharactersAsItems (List<Character> characters)
        {
            foreach (Character c in characters)
            {
                _ShopItems.Add(new ShopItem(c));

                Instantiate(_ShopItemPrefab, _ShopItemContainer).GetComponent<BuyButton>().Init(_ShopItems[^1]);
            }
        }
    }
#endregion
}