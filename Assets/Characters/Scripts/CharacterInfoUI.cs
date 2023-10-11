using UnityEngine;
using UnityEngine.UI;

using CharacterSystem;

namespace UISystem
{
    public class CharacterInfoUI : MonoBehaviour
    {
        public static CharacterInfoUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _CharacterInfoWindow;
        [SerializeField] private Image _ImageCharacter;
        [SerializeField] private Text _TextDMG;
        [SerializeField] private Text _TextSTR, _TextDEX, _TextINT, _TextLUK;
        
        [Header("Displayed Character")]
        [SerializeField] private Character _DisplayedCharacter;

#region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);

            _DisplayedCharacter = null;
        }
#endregion
#region Public Functions
        public void Open ()
        {
            _CharacterInfoWindow.SetActive(true);
            WindowSelector.Select(_CharacterInfoWindow.transform.parent.transform);
        }
        public void Close ()
        {
            _CharacterInfoWindow.SetActive(false);
            EquipmentUI.Instance?.Close();
            SkillManagerUI.Instance?.Close();
        }
        public void Toggle ()
        {
            if (_CharacterInfoWindow.activeSelf) Close();
            else Open();
        }
        
        public void Reset ()
        {
            if (_DisplayedCharacter != null)
                _DisplayedCharacter.OnStatsUpdate -= UpdateInfoUI;

            _ImageCharacter.enabled = false;
            _TextDMG.text = "0 ~ 0";
            _TextSTR.text = "0";
            _TextDEX.text = "0";
            _TextINT.text = "0";
            _TextLUK.text = "0";
        }

        public void DisplayCharacter (Character character)
        {
            if (character == null) Reset();

            if (_DisplayedCharacter != null) _DisplayedCharacter.OnStatsUpdate -= UpdateInfoUI;

            _ImageCharacter.enabled = true;
            _DisplayedCharacter = character;
            _DisplayedCharacter.OnStatsUpdate += UpdateInfoUI;
            UpdateInfoUI(character);
        }
#endregion
#region Private Functions
        private void UpdateInfoUI (Character c)
        {
            if (_DisplayedCharacter != c) return;

            // Change Image
            _ImageCharacter.sprite = c.Base.Sprite;

            // Change Stat Values
            _TextDMG.text = c.LowerDMG + " ~ " + c.UpperDMG;

            _TextSTR.text = string.Concat(c.TotalStats.STR, " (", c.BaseStats.STR, " + ", c.EquipmentStats.STR, ")");
            _TextDEX.text = string.Concat(c.TotalStats.DEX, " (", c.BaseStats.DEX, " + ", c.EquipmentStats.DEX, ")");
            _TextINT.text = string.Concat(c.TotalStats.INT, " (", c.BaseStats.INT, " + ", c.EquipmentStats.INT, ")");
            _TextLUK.text = string.Concat(c.TotalStats.LUK, " (", c.BaseStats.LUK, " + ", c.EquipmentStats.LUK, ")");
        }
#endregion
    }
}