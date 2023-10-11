using System.Collections.Generic;
using UnityEngine;

using AudioSystem;
using CharacterSystem;
using DamageSystem;
using MonsterSystem;

namespace SkillSystem
{
    public class SkillBehaviour : MonoBehaviour
    {   
        [SerializeField] private RectArea _HitBox;
        [SerializeField] private Animator _AnimatorSkill;
        [SerializeField] private SpriteRenderer _Sprite;

        private Character _Character;
        private Monster _Monster;
        private Skill _Skill;
        private SkillBase _Base;
        private int _CurrentNumOfAttacks;

        private List<Collider2D> _Collisions;
        private Dictionary <Collider2D, IDamageable> _HitMonsters;
        private IDamageable _CurrentCollision;

        private ContactFilter2D _Filter;
        private DamageLines _DamageLines;

        private void Awake ()
        {
            _Collisions = new List<Collider2D>();
            _HitMonsters = new Dictionary<Collider2D, IDamageable>();
            _Filter = new ContactFilter2D();
        }

        // Character Used Skill
        public void Init (Character character, Skill skill, float direction)
        {
            _Character = character;
            _Skill = skill;
            _Base = skill.Base;
            _CurrentNumOfAttacks = _Base.NumEnemyHits;

            _Filter.SetLayerMask(GameSettings.EnemyLayer);

            Vector2 temp = _Base.Offset;
            temp.x *= direction;
            _HitBox.Init(temp, _Base.Size);
            
            if (direction == -1) _Sprite.flipX = true;

            CheckSkillType();
        }

        // Monster Used Skill
        public void InitMonster (Monster monster, Skill skill, float direction)
        {
            _Monster = monster;
            _Skill = skill;
            _Base = skill.Base;
            _CurrentNumOfAttacks = _Base.NumEnemyHits;

            _Filter.SetLayerMask(GameSettings.CharacterLayer);

            Vector2 temp = _Base.Offset;
            temp.x *= direction;
            _HitBox.Init(temp, _Base.Size);

            CheckSkillType();
        }
#region Private Functions
        private void CheckSkillType ()
        {
            // Change To Check What Type of Skill is Being Used
            switch (_Base.Type)
            {
                case SkillType.Attack:
                    AudioManager.Instance?.PlayAudio(_Base.UseSFX);
                    Invoke("StartAttackSkill", _Base.CastOffset);
                    break;
                
                case SkillType.DoubleJump:
                    AudioManager.Instance?.PlayAudio(_Base.UseSFX);
                    DetachSkill();
                    _Sprite.sortingLayerName = "Skill Background";
                    _AnimatorSkill.Play(_Base.Name);
                    Invoke ("FinishSkill", _Base.CastDuration);
                    break;

                default:
                    Debug.Log("Have not implemented Other Skill Behaviours");
                    break;
            }
        }

        private void CollisionDetection ()
        {
            // Check If Anything Was Hit
            if (Physics2D.OverlapArea(_HitBox.PointA, _HitBox.PointB, _Filter, _Collisions) < 1) return;

            foreach (Collider2D collision in _Collisions)
            {
                // Check If The Monster / Character Has Already Been Hit By This Skill
                if (!_HitMonsters.TryGetValue(collision, out _CurrentCollision))
                {
                    _HitMonsters.Add(collision, _CurrentCollision);
                    Invoke("PlayHitSFX", 0.2f);
                    
                    _CurrentCollision = collision.GetComponentInParent<IDamageable>();
                    
                    if (_Character != null)
                        _DamageLines = DamageCalculator.CharacterCalculate(_Character, collision.GetComponentInParent<MonsterBehaviour>().Monster, _Skill);
                    else
                        _DamageLines = DamageCalculator.MonsterCalculate(_Monster, collision.GetComponentInParent<CharacterBehaviour>().Character, _Skill);

                    _CurrentCollision.Damage(_DamageLines, ChooseHitEffect());

                    _CurrentNumOfAttacks--;
                    if (_CurrentNumOfAttacks <= 0) FinishSkill();
                }
            }
        }

        private void PlayHitSFX ()
        {
            AudioManager.Instance?.PlayAudio(_Base.HitSFX);
        }

        private void StartAttackSkill ()
        {
            InvokeRepeating("CollisionDetection", 0f, 0.02f);
            Invoke("FinishSkill", _Base.CastDuration);
        }

        private void FinishSkill ()
        {
            CancelInvoke();
            Destroy(gameObject);
        }

        private void DetachSkill ()
        {
            transform.SetParent(null);
        }

        private string ChooseHitEffect ()
        {
            if (_Base.NumHitEffects < 0) return "No Effect";

            if (_Base.NumHitEffects == 0) return string.Concat(_Base.Name, " - Hit Effect");
        
            return string.Concat(_Base.Name, " - Hit Effect ", Random.Range(0, _Base.NumHitEffects));
        }
#endregion
    }
}