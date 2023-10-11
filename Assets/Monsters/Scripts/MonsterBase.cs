using UnityEngine;
using SkillSystem;

namespace MonsterSystem
{
    [CreateAssetMenu(fileName = "MonsterBase", menuName = "Monster/Default")]
    public class MonsterBase : ScriptableObject
    {
        // Memebers
        [field:Header("Description")]
        [field:SerializeField] public int ID {get; private set;}
        [field:SerializeField] public string Name {get; private set;}

        [field:Header("Audio")]
        [field:SerializeField] public AudioClip AttackSFX {get; private set;}
        [field:SerializeField] public AudioClip HitSFX {get; private set;}
        [field:SerializeField] public AudioClip DieSFX {get; private set;}

        [field:Header("Monster Settings")]
        [field:SerializeField] public bool IsBoss {get; private set;}
        [field:SerializeField] public int PhysicalDamageReduction {get; private set;}
        [field:SerializeField] public float AttackDuration {get; private set;}
        [field:SerializeField] public float StaggerDuration {get; private set;}
        [field:SerializeField] public float DeathDuration {get; private set;}

        [field:Header("Monster Skills")]
        [field:SerializeField] public SkillBase BasicAttack {get; private set;}
    }
}