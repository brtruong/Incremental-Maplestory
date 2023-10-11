using System;
using System.Collections.Generic;
using UnityEngine;

using CoreSystems;
using ShopSystem;
using SkillSystem;
using UISystem;

namespace CharacterSystem
{
    public class CharacterManager : MonoBehaviour
    {
        // Memebers
        public static CharacterManager Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        [SerializeField] private GameObject _CharacterPrefab;
        [SerializeField] private List<CharacterBase> _CharacterBases;
        private const string SAVE_FILE = "CharacterData";

        [Header("Characters In Game")]
        [SerializeField] private List<Character> _Characters;

        // Private
        private Dictionary<GameObject, Character> _CharacterTable;
        private Character _ActiveCharacter;
        private CharacterBehaviour _ActiveCharacterBehaviour;

#region Unity Functions
        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);

            _CharacterTable = new Dictionary<GameObject, Character>();
            _ActiveCharacter = null;
        }
#endregion
#region Public Functions
        public void SaveCharacters ()
        {
            _Logger.Log(gameObject, "Saving All Character Data");
            
            foreach (Character c in _Characters)
                c.SavePosition();

            SaveData save = new SaveData(){Characters = _Characters};
            SaveSystem.Save(save, GameSettings.SavePath, SAVE_FILE);
        } 

        public void LoadCharacters ()
        {
            _Logger.Log(gameObject, "Loading All Character Data");

            SaveData save = (SaveData)SaveSystem.Load<SaveData>(GameSettings.SavePath, SAVE_FILE);
            _Characters = save.Characters;

            if (_Characters == null) CreateAllCharacters();
            else AddMissingCharacters();

            ShopUI.Instance?.AddCharactersAsItems(_Characters);
        }

        public void SpawnCharacters ()
        {
            foreach (Character c in _Characters)
                if (c.Level > 0) Spawn(c);
        }

        public void DespawnCharacters ()
        {
            _Logger.Log(gameObject, "Despawning Character Objects");

            CameraManager.Instance?.SetFollowCharacter(null);
            EntitySelectionUI.Instance?.Close(_ActiveCharacter);

            foreach (GameObject o in _CharacterTable.Keys)
                Destroy(o);

            _CharacterTable.Clear();
            _ActiveCharacter = null;
        }
        
        public void SetActiveCharacter (GameObject characterObject)
        {
            if (!_CharacterTable.ContainsKey(characterObject)) return;

            if (_ActiveCharacter != null)
            {
                // Deactivate Old
                _Logger.Log(gameObject, "Deactivating {" + _ActiveCharacter.Name + "}");
                _ActiveCharacterBehaviour.Deactivate();
                QuickSlotManager.Instance?.ResetSkillQuickSlots();
                
                if (_ActiveCharacter == _CharacterTable[characterObject])
                {
                    // Same Character
                    EntitySelectionUI.Instance?.Close(_ActiveCharacter);
                    CameraManager.Instance?.SetFollowCharacter(null);
                    _ActiveCharacter = null;
                    return;
                }
            }

            // Activate New
            _ActiveCharacterBehaviour = characterObject.GetComponent<CharacterBehaviour>();
            _ActiveCharacterBehaviour.Activate();
            
            _ActiveCharacter = _CharacterTable[characterObject];
            CharacterInfoUI.Instance?.DisplayCharacter(_ActiveCharacter);
            EquipmentUI.Instance?.ChangeDisplayedEquipment(_ActiveCharacter);
            SkillManagerUI.Instance?.ChangeDisplayedSkillTab(_ActiveCharacter);
            CameraManager.Instance?.SetFollowCharacter(characterObject.transform);
            EntitySelectionUI.Instance?.Open(_ActiveCharacter);
        }

        public void CheckCharacterChanges ()
        {
            foreach (Character c in _Characters)
            {
                if (c.Level <= 0) continue;

                if (!_CharacterTable.ContainsValue(c)) Spawn(c);
            }
        } 
#endregion
#region Private Functions
        private void Spawn (Character character)
        {
            _Logger.Log(gameObject, "Spawning Character {" + character.Name + "}");

            Transform t = Instantiate(_CharacterPrefab, transform).transform;

            t.GetComponent<CharacterBehaviour>().Init(character);
            character.SetTransform(t);
            if (character.PosX == -1f && character.PosY == -1f)
                t.position = GameSettings.SpawnLocation;
            else
                t.position = new Vector3 (character.PosX, character.PosY, 0f);

            t.gameObject.name = string.Concat(character.Name, " - Object");
            _CharacterTable.Add(t.gameObject, character);

            EquipmentUI.Instance?.GenerateEquipmentUI(character);
            SkillManagerUI.Instance?.GenerateSkillTab(character);
            CharacterInfoUI.Instance?.DisplayCharacter(character);
        }

        private void CreateAllCharacters ()
        {
            _Logger.Log(gameObject, "Creating All Characters (New File)");

            _Characters = new List<Character>();
            foreach (CharacterBase characterBase in _CharacterBases)
                _Characters.Add(new Character(characterBase));
        }

        private void AddMissingCharacters ()
        {
            _Logger.Log(gameObject, "Adding Missing Characters");

            foreach (Character c in _Characters)
                c.UpdateBase();

            List<Character> temp = new List<Character>();
            foreach (Character character in _Characters)
                temp.Add(character);

            _Characters.Clear();

            foreach (CharacterBase characterBase in _CharacterBases)
            {
                bool exists = false;
 
                for (int i = 0; i < temp.Count; i++) // Check if Character does not exists
                {
                    if (characterBase.ID != temp[i].Base.ID) continue;

                    // Character Already Exists Add it back
                    _Characters.Add(temp[i]);
                    temp.RemoveAt(i);
                    exists = true;
                    break;
                }

                if (!exists) _Characters.Add(new Character(characterBase)); // Character doesn't exist create it
            }
        }

        private struct SaveData 
        {
            public List<Character> Characters;
        }
#endregion
    }
}
