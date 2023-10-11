using UnityEngine;
using SkillSystem;

namespace CharacterSystem
{
    [CreateAssetMenu(fileName = "New CharacterBase", menuName = "Characters/Default")]
    public class CharacterBase : ScriptableObject
    {
        [field:Header ("Settings")]
        [field:SerializeField] public int ID {get; private set;}
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField] public StatType MainStat {get; private set;}
        [field:SerializeField] public RuntimeAnimatorController AnimatorController {get; private set;}
        [field:SerializeField] public Sprite Sprite {get; private set;}
        [field:SerializeField] public Sprite SelectionSprite {get; private set;}
        [field:SerializeField] public Sprite ShopIcon {get; private set;}

        [field:Header ("Skills")]
        [field:SerializeField] public SkillBase[] SkillBases;

        public void ChangeID (int newID)
        {
            if (newID >= 0) ID = newID;
        }
    }

}