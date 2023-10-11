using UnityEngine;
using UnityEngine.UI;
using CoreSystems;
using CraftingSystem;
using ShopSystem;
using SkillSystem;

namespace UISystem
{
    public class RootUIManager : MonoBehaviour
    {
        // Members
        public static RootUIManager Instance {get; private set;}
        
        [Header ("Title Scene Windows")]
        [SerializeField] private GameObject _TitleScene;
        [SerializeField] private GameObject _SaveSelectionWindow;
        [SerializeField] private Text[] _TextSaveSlot;

        [Header ("In Game Windows")]
        [SerializeField] private GameObject _InGameUI;
        [SerializeField] private GameObject _PauseMenu;

    #region Unity Functions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);
        }

        private void Start ()
        {
            CloseAllUI();
            OpenTitleSceneUI();
        }
    #endregion
    #region Public Functions
        public void StartGame ()
        {
            CloseAllUI();
            _InGameUI.SetActive(true);
        }

        public void QuitGame ()
        {
            GameManager.Instance?.QuitGame();
        }
        
        public void ExitToTitle ()
        {
            CloseAllUI();
            GameManager.Instance?.ExitToTitle();
            OpenTitleSceneUI();
        }

        public void UpdateSaveSlotUI (GameData[] gameDatas)
        {
            for (int i = 1; i <= 3; i++)
            {
                GameData g = gameDatas[i];
                if (g == null) continue;

                _TextSaveSlot[i].text = 
                "Save Slot " + i +
                "\nStage: " + g.Stage.Level +
                "\nMeso: " + g.Meso.Amount;
            }
        }

        public void OpenSaveSelectionWindow () => _SaveSelectionWindow.SetActive(true);
        public void CloseSaveSelectionWindow () => _SaveSelectionWindow.SetActive(false);

        public void OpenSaveFile1 () => GameManager.Instance?.LoadSaveFile(1);
        public void OpenSaveFile2 () => GameManager.Instance?.LoadSaveFile(2);
        public void OpenSaveFile3 () => GameManager.Instance?.LoadSaveFile(3);

        public void OpenMenu () => _PauseMenu.SetActive(true);
        public void CloseMenu () => _PauseMenu.SetActive(false);
        public void ToggleMenu () => _PauseMenu.SetActive(!_PauseMenu.activeSelf);
    #endregion
    #region Private Functions
        private void CloseAllUI ()
        {
            CloseTitleSceneUI();
            CloseAllInGameUI();
        }

        private void CloseAllInGameUI ()
        {
            _PauseMenu.SetActive(false);
            InventoryUI.Instance?.Close();
            CharacterInfoUI.Instance?.Close();
            CraftingUI.Instance?.Close();
            EquipmentUI.Instance?.Close();
            ItemDisplayUI.Instance?.Close();
            ShopUI.Instance?.Close();
            SkillManagerUI.Instance?.Close();
            _InGameUI.SetActive(false);
        }

        private void CloseTitleSceneUI ()
        {
            _TitleScene.SetActive(false);
            _SaveSelectionWindow.SetActive(false);
        }

        private void OpenTitleSceneUI ()
        {
            _TitleScene.SetActive(true);
        }
    #endregion
    }
}