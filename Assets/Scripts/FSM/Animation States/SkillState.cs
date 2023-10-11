using System;
using MovementAndAnimation;
using SkillSystem;

namespace StateSystem
{
    public class SkillState : BaseState
    {
        public event HandleUseSkill OnUseSkill;
        public delegate void HandleUseSkill (SkillBase b);

        private SpriteController _Controller;
        private bool _ContinueSkill;
        private SkillBase _CurrentSkill;
        private DateTime _TimeNextSkillCast;

        public SkillState (SpriteController controller)
        {
            _Controller = controller;
            _ContinueSkill = false;
            _CurrentSkill = null;
        }

        public override void EnterState ()
        {
            _ContinueSkill = true;
            _CurrentSkill = _Controller.QueuedSkill;
            CastSkill(_CurrentSkill);
        }

        public override void HandleState ()
        {
            _CurrentSkill = _Controller.QueuedSkill;

            if (_TimeNextSkillCast.CompareTo(DateTime.Now) <= 0)
            {
                if (_CurrentSkill != null) CastSkill(_CurrentSkill);
                else _ContinueSkill = false;
            }
        }
        
        public override void ExitState (){}

        public override Type CheckSwitchState ()
        {
            // Check If Dead
            if (_Controller.IsDead) return typeof(DeadState);

            // Check If Staggered
            if (_Controller.IsStaggered) return typeof(StaggerState);

            // Check If Still In Skill
            if (_ContinueSkill) return this.GetType();
            
            // Check To ClimbRope
            if (_Controller.VerticalInput != 0 && _Controller.RopeCollision) return typeof(ClimbingState);

            // Check If Falling
            if (_Controller.GroundCollision == null) return typeof(FallingState);

            // Check For Jump Input
            if (_Controller.JumpInput) return typeof(JumpingState);

            // Check For Horizontal Movement
            if (_Controller.HorizontalInput != 0) return typeof(WalkingState);

            // Check For Prone
            if (_Controller.VerticalInput == -1) return typeof(ProneState);
            
            return typeof(IdleState);
        }

        private void CastSkill (SkillBase skill)
        {
            OnUseSkill?.Invoke(skill);
            _TimeNextSkillCast = DateTime.Now.AddMilliseconds(skill.ActionDelay * 1000);
            
            if (_Controller.QueuedSkill.Type == SkillType.DoubleJump)
            {
                _ContinueSkill = false;
                _Controller.DequeueSkill();
                return;
            }

            _Controller.AnimatorSprite.Play(skill.Name);
        }
    }
}