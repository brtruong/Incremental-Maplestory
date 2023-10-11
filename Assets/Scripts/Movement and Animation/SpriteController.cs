using System;
using System.Collections.Generic;
using UnityEngine;
using DamageSystem;
using StateSystem;
using SkillSystem;

namespace MovementAndAnimation
{
    public enum SpriteDirection {Left = 1, Right = -1}

    public class SpriteController : MonoBehaviour
    {
        // Memebers
        [Header("Physics Settings")]
        [SerializeField] private Rigidbody2D _Rb;
        [SerializeField] private RectArea _GroundCheckArea, _RopeCheckArea;
        [SerializeField] private float _MovementSpeed = 2f;
        [SerializeField] private float _ClimbingSpeed = 1f;
        [SerializeField] private float _JumpForce = 4f;

        [Header("Sprite Settings")]
        [SerializeField] private SpriteDirection _SpriteDirection = SpriteDirection.Left;
        [SerializeField] private SpriteRenderer _Sprite, _SpriteSkillEffect;
        [SerializeField] private Animator _AnimatorSprite, _AnimatorHitEffect; 

        private Collider2D _GroundCollion, _RopeCollision;
        private int _HorizontalInput, _VerticalInput;
        [SerializeField] private bool _JumpInput;

        private SkillBase _QueuedSkill;
        private bool _IsStaggered, _IsDead;

        private Dictionary<Type, BaseState> _StateTable;
        private StateMachine _StateMachine;

        // Getters
        public Rigidbody2D Rb => _Rb;
        public Collider2D GroundCollision => _GroundCollion;
        public Collider2D RopeCollision => _RopeCollision;

        public Animator AnimatorSprite => _AnimatorSprite;
        public Animator AnimatorHitEffect => _AnimatorHitEffect;
        public SpriteDirection SpriteDirection => _SpriteDirection;

        public int HorizontalInput => _HorizontalInput;
        public int VerticalInput => _VerticalInput;
        public bool JumpInput => _JumpInput;
        public float MovementSpeed => _MovementSpeed;
        public float ClimbingSpeed => _ClimbingSpeed;
        public float JumpForce => _JumpForce;

        public SkillBase QueuedSkill => _QueuedSkill;
        public bool CanDoubleJump, AllowDoubleJump;
        public bool IsStaggered => _IsStaggered;
        public bool IsDead => _IsDead;

        public FallingState FallingState => (FallingState) _StateTable[typeof(FallingState)];
        public SkillState SkillState => (SkillState) _StateTable[typeof(SkillState)];

        private void Awake ()
        {
            IDamageable d = gameObject.GetComponent<IDamageable>();
            d.OnStagger += StaggerEvent;
            d.OnDamage += DamageEvent;

            _StateTable = new Dictionary<Type, BaseState>()
            {
                {typeof(IdleState), new IdleState(this)},
                {typeof(WalkingState), new WalkingState(this)},
                {typeof(JumpingState), new JumpingState(this)},
                {typeof(FallingState), new FallingState(this)},                
                {typeof(ProneState), new ProneState(this)},
                {typeof(ClimbingState), new ClimbingState(this)},
                {typeof(StaggerState), new StaggerState(this)},
                {typeof(DeadState), new DeadState(this)},
                {typeof(SkillState), new SkillState(this)}
            };
            
            _IsDead = _IsStaggered = AllowDoubleJump = false;
            _QueuedSkill = null; 
            _Sprite.sortingOrder = transform.GetSiblingIndex() + 1;
        }

        private void Start () => _StateMachine = new StateMachine(_StateTable);
        private void Update () => _StateMachine.Update();
        private void FixedUpdate ()
        {
            _GroundCollion = Physics2D.OverlapArea(_GroundCheckArea.PointA, _GroundCheckArea.PointB, GameSettings.GroundLayer);
            _RopeCollision = Physics2D.OverlapArea(_RopeCheckArea.PointA, _RopeCheckArea.PointB, GameSettings.RopeLayer);
        }
#region Public Functions
        public void MoveHorizontal (int direction)
        {
            if (direction == 0 || direction == 1 || direction == -1)
                _HorizontalInput = direction;
        }

        public void MoveVertical (int direction)
        {
            if (direction == 0 || direction == 1 || direction == -1)
                _VerticalInput = direction;
        }
        public void Jump (bool jump) => _JumpInput = jump;

        public void FlipSprite ()
        {
            _SpriteDirection = (SpriteDirection)(-1 * (int)_SpriteDirection);
            _Sprite.flipX = !_Sprite.flipX;
            _SpriteSkillEffect.flipX = !_SpriteSkillEffect.flipX;
        }

        public void DisplayFirst () => _Sprite.sortingOrder = 100;
        public void DisplayNormal () => _Sprite.sortingOrder = transform.GetSiblingIndex() + 1;

        public void DequeueSkill () => _QueuedSkill = null;
        public void QueueSkill (SkillBase skillBase)
        {
            if (skillBase == null) return;

            _QueuedSkill = skillBase;
            Invoke("DequeueSkill", GameSettings.SkillQueueTime);
        }
#endregion
#region Private Functions
        private void DamageEvent (object sender, OnDamageArgs e)
        {
            _AnimatorHitEffect.Play(e.DamageEffect);
            _IsDead = e.IsDead;
        }
        
        private void StaggerEvent (object sender, OnStaggerArgs e)
        {
            _IsStaggered = true;
            Invoke("EndStagger", e.StaggerTime);
        }
        
        private void EndStagger ()
        {
            _IsStaggered = false;
        }
#endregion
    }
}