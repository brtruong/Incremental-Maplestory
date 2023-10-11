using UnityEngine;
using UnityEngine.UI;

using CharacterSystem;
using DamageSystem;
using MonsterSystem;

namespace UISystem
{
    public class EntitySelectionUI : MonoBehaviour
    {
        // Members
        public static EntitySelectionUI Instance {get; private set;}

        [SerializeField] private GameObject _MainWindow;
        [SerializeField] private Image _EntityImage;
        [SerializeField] private Text _LevelText, _HPText, _MPText;
        [SerializeField] private Slider _HPSlider, _MPSlider;

        private Character _Character;
        private IDamageable _CurrentEntity;

        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }

        public void Close (Character character)
        {
            if (_Character != null) _Character.OnUpdate -= UpdateCharacter;
            _MainWindow.SetActive(false);
        }
        
        public void Open (Character character)
        {
            if (character == null) return;

            if (_Character != null) _Character.OnUpdate -= UpdateCharacter;
            _Character = character;
            _Character.OnUpdate += UpdateCharacter;

            _EntityImage.sprite = _Character.Base.SelectionSprite;
            UpdateCharacter(_Character);            
            
            _MainWindow.SetActive(true);
        }

        public void UpdateCharacter (Character character)
        {
            _HPText.text = character.HP.ToString();
            _HPSlider.value = (float) character.HP / character.TotalStats.MaxHP;

            _MPText.text = character.MP.ToString();
            _MPSlider.value = (float) character.MP / character.TotalStats.MaxMP;

            _LevelText.text = string.Concat("Lvl : ", character.Level);
        }
    }
}