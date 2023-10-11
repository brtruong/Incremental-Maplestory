using UnityEngine;

namespace SkillSystem
{
    public enum SkillType {Attack, Projectile, DoubleJump, Passive}

    [CreateAssetMenu(fileName = "New SkillBase", menuName = "Skills/Default")]
    public class SkillBase : ScriptableObject
    {
        [field:Header("Skill Base Settings")]
        [field:SerializeField] public int ID {get; private set;}
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField] public SkillType Type {get; private set;}
        [field:SerializeField] public Sprite Icon {get; private set;}
        [field:SerializeField] public string Description {get; private set;}

        [field:Header("Others")]
        [field:SerializeField] public int NumHitEffects {get; private set;}
        [field:SerializeField] public float CastOffset {get; private set;}
        [field:SerializeField] public float CastDuration {get; private set;}
        [field:SerializeField] public float ActionDelay {get; private set;}      

        [field:Header("SFX")]
        [field:SerializeField] public AudioClip UseSFX {get; private set;}
        [field:SerializeField] public AudioClip HitSFX {get; private set;}

        [field:Header("Hit Box")]
        [field:SerializeField] public Vector2 Offset {get; private set;}
        [field:SerializeField] public Vector2 Size {get; private set;}

        [field:Header("Attack Skill Values")]
        [field:SerializeField] public int NumHits {get; private set;}
        [field:SerializeField] public int NumEnemyHits {get; private set;}
        [field:SerializeField] public int BasePctDMG {get; private set;}
        [field:SerializeField] public int PerLvlPctDMG {get; private set;}

        public void ChangeID (int newID)
        {
            if (newID > 0) ID = newID;
        }
    }
}