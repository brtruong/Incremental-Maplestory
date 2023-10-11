using System;
using UnityEngine;
using AudioSystem;
using DamageSystem;
using MapSystem;
using MesoSystem;
using MovementAndAnimation;
using SkillSystem;

namespace MonsterSystem
{
    public class MonsterBehaviour : MonoBehaviour, IDamageable
    {
        // Members
        [Header("Settings")]
        [SerializeField] private SpriteController _Controller;
        [SerializeField] private GameObject _SkillPrefab;
        [SerializeField] private GameObject _HitBox;
        [SerializeField] private MonsterAI _AI;

        [Header("Monster Info")]
        [SerializeField] private Monster _Monster;

        private int _TotalDamage;
        private OnDamageArgs _OnDamageArgs;

        public Monster Monster => _Monster;

        public event EventHandler<OnDamageArgs> OnDamage;
        public event EventHandler<OnStaggerArgs> OnStagger;

#region Public Functions
        public void Init (MonsterBase monsterBase, int level)
        {
            _Controller.SkillState.OnUseSkill += UseSkillEvent;

            _Monster = new Monster(monsterBase, level);
            _OnDamageArgs = new OnDamageArgs();
            _AI.enabled = true;
            _HitBox.SetActive(true);
        }

        public void Damage (DamageLines damageLines, string hitEffect)
        {
            if (_Monster.IsDead) return;

            _TotalDamage = damageLines.Sum;
            _Monster.Damage(_TotalDamage);

            UpdateOnDamageArgs();
            _OnDamageArgs.DamageLines = damageLines;
            _OnDamageArgs.DamageEffect = hitEffect;
            OnDamage?.Invoke(this, _OnDamageArgs);

            StaggerCheck(_TotalDamage);
            DeathCheck();
        }

        [ContextMenu("Attack")]
        public void Attack ()
        {
            _Controller.QueueSkill(_Monster.Base.BasicAttack);
        }
#endregion
#region Private Functions
        private void UpdateOnDamageArgs ()
        {
            _OnDamageArgs.HP = _Monster.HP;
            _OnDamageArgs.MaxHP = _Monster.MaxHP;
            _OnDamageArgs.IsDead = _Monster.IsDead;
        }

        private void StaggerCheck (int damage)
        {
            if (_Monster.IsDead || damage < 0.2 * _Monster.MaxHP) return;

            AudioManager.Instance?.PlayAudio(_Monster.Base.HitSFX);
            OnStagger?.Invoke(this, new OnStaggerArgs {StaggerTime = _Monster.Base.StaggerDuration});
        }

        private void DeathCheck ()
        {
            if (!_Monster.IsDead) return;

            _HitBox.SetActive(false);
            
            StageManager.Instance?.MonsterDied(gameObject);
            AudioManager.Instance?.PlayAudio(_Monster.Base.DieSFX);
            Invoke("DestroyMonster", _Monster.Base.DeathDuration);
        }
        
        private void DestroyMonster ()
        {
            MesoManager.Instance?.SpawnMeso(transform.position, transform.rotation, _Monster.MesoDropAmount);
            Destroy(gameObject);
        }

        private void UseSkillEvent (SkillBase skillBase)
        {
            Instantiate(_SkillPrefab, transform).GetComponent<SkillBehaviour>().InitMonster(_Monster, _Monster.BasicAttack, transform.localScale.x);
        }
#endregion
    }
}