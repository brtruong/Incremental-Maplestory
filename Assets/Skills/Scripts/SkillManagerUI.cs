using System.Collections.Generic;
using UnityEngine;

using CharacterSystem;
using SkillSystem;

namespace UISystem
{
    public class SkillManagerUI : MonoBehaviour
    {
        public static SkillManagerUI Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private GameObject _Window;
        [SerializeField] private Transform _Container;
        [SerializeField] private GameObject _PrefabSkillTab;
        [SerializeField] private GameObject _PrefabSkillSlot;

        private Dictionary <Character, GameObject> _TabTable;
        private Character _ActiveCharacter;

        public void Open ()
        {
            _Window.SetActive(true);
            EquipmentUI.Instance?.Close();
        }
        public void Close () => _Window.SetActive(false);
        public void Toggle ()
        {
            if (_Window.activeSelf) Close();
            else Open();
        }

#region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);

            _TabTable = new Dictionary<Character, GameObject>();
            _ActiveCharacter = null;
        }
#endregion
#region Public Functions
        public void GenerateSkillTab (Character newCharacter)
        {
            GameObject newSkillTab = Instantiate(_PrefabSkillTab, _Container);
            newSkillTab.name = newCharacter.Base.Name + " - Skill Tab";

            foreach (Skill s in newCharacter.Skills)
                Instantiate(_PrefabSkillSlot, newSkillTab.transform).GetComponentInChildren<SkillSlotButton>().Init(s);

            _TabTable.Add(newCharacter, newSkillTab);
            newSkillTab.SetActive(false);
        }

        public void RemoveAllWindows ()
        {
            foreach (GameObject o in _TabTable.Values)
                Destroy(o);

            _TabTable.Clear();
            _ActiveCharacter = null;
        }

        public void ChangeDisplayedSkillTab (Character character)
        {
            if (character == null) return;

            // If Any - Deactivate Skill Tab of previous active character
            if (_ActiveCharacter != null) _TabTable[_ActiveCharacter].SetActive(false);
                
            // If Skill Tab for character does not exist create it
            if (!_TabTable.ContainsKey(character)) GenerateSkillTab(character);
            
            _TabTable[character].SetActive(true);
            _ActiveCharacter = character;
        }
#endregion
    }
}   