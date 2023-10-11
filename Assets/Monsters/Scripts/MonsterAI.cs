using System;
using System.Collections.Generic;
using UnityEngine;
using MovementAndAnimation;
using StateSystem;

namespace MonsterSystem
{
    public class MonsterAI : MonoBehaviour
    {
        // Memebers 
        [Header("Settings")]
        [SerializeField] private SpriteController _Controller;
        [SerializeField] private MonsterBehaviour _MonsterBehaviour;
        [SerializeField] private RectArea _DetectionArea, _AttackArea; 

        private StateMachine _StateMachine;

        public Collider2D TargetTracking {get; private set;}
        public Collider2D TargetInRange {get; private set;}

        private void Awake ()
        {
            TargetTracking = TargetInRange = null;

            Dictionary<Type, BaseState> stateTable = new Dictionary<Type, BaseState>()
            {
                {typeof(StillState), new StillState(_Controller, this)},
                {typeof(WanderState), new WanderState(_Controller, this)},
                {typeof(TrackingState), new TrackingState(_Controller, this)},
                {typeof(AttackingState), new AttackingState(_Controller, this, _MonsterBehaviour)}
            };

            _StateMachine = new StateMachine(stateTable);
        }

        private void OnEnable ()
        {
            TargetTracking = TargetInRange = null;
        }

        private void Start ()
        {
            _AttackArea.Init(_MonsterBehaviour.Monster.BasicAttack.Base.Offset, _MonsterBehaviour.Monster.BasicAttack.Base.Size);
        }

        private void Update ()
        {
            _StateMachine.Update();
        }

        private void FixedUpdate()
        {
            TargetTracking = Physics2D.OverlapArea(_DetectionArea.PointA, _DetectionArea.PointB, GameSettings.CharacterLayer);
            TargetInRange = Physics2D.OverlapArea(_AttackArea.PointA, _AttackArea.PointB, GameSettings.CharacterLayer);
        }
    }
}