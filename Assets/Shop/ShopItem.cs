using UnityEngine;

using CharacterSystem;
using MesoSystem;
using SkillSystem;

namespace ShopSystem
{
    public enum ShopItemType
    {
        Character, Skill
    }

    // Shop Item Holds things that can be leveled up
    [System.Serializable]
    public class ShopItem
    {
        // Members
        [field:SerializeField] public int Level {get; private set;}
        [field:SerializeField] public int Cost {get; private set;}

        private Character _Character;
        private Skill _Skill;

        public string Name {get; private set;}
        public Sprite Icon {get; private set;}
        public string Description {get; private set;}
        public ShopItemType Type {get; private set;}

        public ShopItem (Character character)
        {
            Type = ShopItemType.Character;
            _Character = character;
            Name = _Character.Base.Name;
            Icon = _Character.Base.ShopIcon;
            Level = character.Level;
            Cost = Level * 10;
        }

        public ShopItem (Skill skill)
        {
            Type = ShopItemType.Skill;
            _Skill = skill;
            Level = skill.Level;
            Cost = Level * 10;
        }

        public void BuyLevel ()
        {
            if (!MesoManager.Instance.MinusMeso(Cost)) return;

            switch (Type)
            {
                case ShopItemType.Character:
                    _Character.LevelUp();
                    Level = _Character.Level;
                    Cost = Level * 10;
                    CharacterManager.Instance?.CheckCharacterChanges();
                    break;
                
                case ShopItemType.Skill:
                    _Skill.LevelUp();
                    Level = _Skill.Level;
                    Cost = Level * 10;
                    break;
                
                default:
                    break;
            }
        }
    }
}