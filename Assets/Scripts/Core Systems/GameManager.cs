using UnityEngine;

using CharacterSystem;
using InventorySystem;
using MapSystem;
using MesoSystem;
using ShopSystem;
using SkillSystem;
using UISystem;

namespace CoreSystems
{
    public class GameManager : MonoBehaviour
    {
        // Members
        public static GameManager Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        [SerializeField] private SaveSystem _SaveSystem;
        [SerializeField] private bool _UsePreLoadedGame;
        
        [Header("Game Data")]
        [SerializeField] private GameData[] _GameDatas;

        // Private
        private int _CurrentSaveFile;
        private GameData _GameData => _GameDatas[_CurrentSaveFile];

#region Unity Functions
        private void Awake ()
        {
            Configure();
        }

        private void Start ()
        {
            RootUIManager.Instance?.UpdateSaveSlotUI(_GameDatas);
        }
#endregion
#region Public Functions
        public void LoadSaveFile (int num)
        {
            _Logger.Log(gameObject, "Loading Save File {" + num + "}");

            _CurrentSaveFile = num;
            if (_GameDatas[num] == null) CreateNewSaveFile(num);

            LoadGameState();
            SceneController.Instance?.LoadScene("Level 1");
        }

        public void ExitToTitle ()
        {
            _Logger.Log(gameObject, "Exiting To Title Screen");
            
            SaveGame();
            CameraManager.Instance?.Deactivate();
            RootUIManager.Instance?.UpdateSaveSlotUI(_GameDatas);
            EquipmentUI.Instance?.RemoveAllWindows();
            InventoryUI.Instance?.RemoveAllWindows();
            SkillManagerUI.Instance?.RemoveAllWindows();
            ShopUI.Instance?.Clear();
            
            StageManager.Instance?.StopSpawn();
            CharacterManager.Instance?.DespawnCharacters();
        }

        public void StartGame ()
        {
            _Logger.Log(gameObject, "Starting Game");

            RootUIManager.Instance?.StartGame();
            CameraManager.Instance?.Activate();
            StageManager.Instance?.StartSpawn();
            CharacterManager.Instance?.SpawnCharacters();
            CharacterInfoUI.Instance?.Reset();
        }
        
        public void ResumeGame ()
        {
            _Logger.Log(gameObject, "Resuming Game {Not Implemented Yet}");
        }

        public void PauseGame ()
        {
            _Logger.Log(gameObject, "Pausing Game {Not Implemented Yet}");
        }

        public void QuitGame ()
        {
            _Logger.Log(gameObject, "Quiting Game");

            Application.Quit();
        }
#endregion
#region Private Functions
        private void Configure ()
        {
            if (!Instance)
                Instance = this;
            else 
                Destroy(gameObject);

            _GameDatas = new GameData[4] {null, null, null, null};

            _GameDatas[1] = SaveSystem.Load<GameData>("SaveSlot1/", "Data_1");
            _GameDatas[2] = SaveSystem.Load<GameData>("SaveSlot2/", "Data_2");
            _GameDatas[3] = SaveSystem.Load<GameData>("SaveSlot3/", "Data_3");
        }

        private void SaveGame ()
        {
            _Logger.Log(gameObject, "Saving Game");

            InventoryManager.Instance?.SaveInventories();
            CharacterManager.Instance?.SaveCharacters();
            SaveSystem.Save(_GameData, GameSettings.SavePath, string.Concat("Data_", _CurrentSaveFile));
        }

        private void CreateNewSaveFile (int num)
        {
            _Logger.Log(gameObject, "Creating New Save File {" + num + "}");
            
            _GameDatas[num] = new GameData();
            SaveSystem.Save(_GameDatas[num], GameSettings.SavePath, string.Concat("Data_", _CurrentSaveFile));
        }

        private void LoadGameState ()
        {
            GameSettings.Instance?.SetSavePath(string.Concat("SaveSlot", _CurrentSaveFile, "/"));

            MesoManager.Instance?.LoadMeso(_GameData);
            StageManager.Instance?.LoadStage(_GameData.Stage);
            InventoryManager.Instance?.LoadInventories();
            CharacterManager.Instance?.LoadCharacters();
        }

#endregion
    }    
}
