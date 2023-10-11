using System;
using System.Collections.Generic;
using UnityEngine;

using DamageSystem;
using MovementAndAnimation;
using SkillSystem;
using UISystem;

namespace CharacterSystem
{
	public class CharacterBehaviour : MonoBehaviour, IDamageable
	{
		// Memebers
		[Header("Settings")]
		[SerializeField] private CharacterInput _CharacterInput;
		[SerializeField] private SpriteController _Controller;
		[SerializeField] private GameObject _SpriteHitBox;
		[SerializeField] private Transform _SkillOrigin;
		[SerializeField] private GameObject _SkillPrefab;

		[Header("Character Info")]
		[SerializeField] private Character _Character;

		private Skill _UseSkill;
		private GameObject _SkillObject;

		private Dictionary<SkillBase, Skill> _UseableSkillTable;
		private Dictionary<string, Skill> _KeybindTable;
		private OnDamageArgs _DamageArgs;
		private bool _IsDead;

		public Character Character => _Character;

		public event EventHandler<OnDamageArgs> OnDamage;
        public event EventHandler<OnStaggerArgs> OnStagger;
#region Unity Functions
		private void Start ()
		{
			_Controller.SkillState.OnUseSkill += UseSkillEvent;
			_DamageArgs = new OnDamageArgs();
			_DamageArgs.MaxHP = 100;
			_DamageArgs.HP = 100;
			Deactivate();
		}

		private void OnDisable ()
		{
			foreach (Skill s in _Character.Skills)
				s.OnUpdate -= HandleSkillUpdate;
		}
#endregion
#region Public Functions
		public void Init (Character character)
		{
			_Character = character;
			_Controller.AnimatorSprite.runtimeAnimatorController = _Character.Base.AnimatorController;

			_UseableSkillTable = new Dictionary<SkillBase, Skill>();
			_KeybindTable = new Dictionary<string, Skill>();

			foreach (Skill s in _Character.Skills)
			{
				s.OnUpdate += HandleSkillUpdate;

				if (s.Level > 0) _UseableSkillTable.Add(s.Base, s);
				if (s.Keybind != "None") _KeybindTable.Add(s.Keybind, s);
				if (s.Base.Type == SkillType.DoubleJump && s.Level > 0)
				{
					_Controller.AllowDoubleJump = true;
					_Controller.FallingState.SetDoubleJump(s.Base);
				}
			}

			_SpriteHitBox.SetActive(true);
		}

		public void Activate ()
		{
			_CharacterInput.Activate();
			_Controller.DisplayFirst();
			
			foreach (var entry in _KeybindTable)
				QuickSlotManager.Instance?.UpdateQuickSlot(entry.Key, entry.Value);
		}

		public void Deactivate ()
		{
			_CharacterInput.Deactivate();
			_Controller.MoveHorizontal(0);
			_Controller.MoveVertical(0);
			_Controller.Jump(false);
			_Controller.DisplayNormal();
		}

		public void UseSkill (string key)
		{
			if (_KeybindTable.ContainsKey(key))
				_Controller.QueueSkill(_KeybindTable[key].Base);
		}

		public void Damage (DamageLines damageLines, string hitEffect)
		{
			if (_IsDead) return;

			_DamageArgs.DamageLines = damageLines;
			_IsDead = _Character.Damage(damageLines.Sum);
			OnDamage?.Invoke(this, _DamageArgs);

			if (!_IsDead) return;
			_DamageArgs.IsDead = true;
			OnDamage?.Invoke(this, _DamageArgs);
			Invoke("Respawn", GameSettings.RespawnTime);
			_SpriteHitBox.SetActive(false);
		}
#endregion
#region Private Functions
		private void Respawn ()
		{
			Character.RecoverHP(_Character.TotalStats.MaxHP);
			Character.RecoverMP(_Character.TotalStats.MaxMP);
			_DamageArgs.IsDead = false;
			_IsDead = false;
			OnDamage?.Invoke(this, _DamageArgs);
			_SpriteHitBox.SetActive(true);
		}

		private void UseSkillEvent (SkillBase skillBase)
		{
			if (_UseableSkillTable.TryGetValue(skillBase, out _UseSkill))
				Instantiate(_SkillPrefab, _SkillOrigin).GetComponent<SkillBehaviour>().Init(_Character, _UseSkill, (int)_Controller.SpriteDirection);
		}

		private void HandleSkillUpdate (Skill skill)
		{
			// Skill Level 0 -> 1
			if (skill.Level > 0 && !_UseableSkillTable.ContainsValue(skill))
			{
				_UseableSkillTable.Add(skill.Base, skill);
				if (skill.Base.Type == SkillType.DoubleJump)
				{
					_Controller.AllowDoubleJump = true;
					_Controller.FallingState.SetDoubleJump(skill.Base);
				}
				return;
			}

			if (!_KeybindTable.ContainsValue(skill))
			{
				// Skill not in KeybindTable && No Keybind Conflict
				if (skill.Keybind != "None")
				{
					if (!_KeybindTable.ContainsKey(skill.Keybind))
					{
						Debug.Log("Adding to {" + skill.Keybind + "} No Conflict");
						_KeybindTable.Add(skill.Keybind, skill);
					}	
					else
					{
						Debug.Log("Conflict Reseting Skill to None");
						skill.SetKeybind("None");
					}
				}
				return;
			}
			else
			{
				// Skill is in KeybindTable
				foreach (var entry in _KeybindTable)
				{
					if (entry.Value != skill) continue;

					// No Change
					if (entry.Key == skill.Keybind) return;

					// No Keybind Conflict - Remove Old and Add New
					if (!_KeybindTable.ContainsKey(skill.Keybind))
					{
						_KeybindTable.Remove(entry.Key);
						_KeybindTable.Add(skill.Keybind, skill);
						return;
					}

					// Keybind Conflict
					skill.SetKeybind("None");
					return;
				}
			}

		}
#endregion
	}
}
