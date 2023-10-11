using System.Collections.Generic;
using UnityEngine;
using DamageSystem;
using SkillSystem;

namespace UISystem
{
    public class DamageLineUI : MonoBehaviour
    {
        [SerializeField] private bool _IsActive;
        [SerializeField] private GameObject _DamageLinePrefab;
        [SerializeField] private DamageSkin _DamgeSkin;

        private const float TTL = 0.8f;
        private const float LINE_DELAY = 0.08f;
        private const float LINE_SPACE = 0.25f;
        private const float NUM_SPACE = 0.08f;

        private DamageLines _LinesWaiting;
        private List<Transform> _LineObjects;

        private GameObject _NextNum;
        private Transform _NextNumTransform;
        private Vector3 _RunningPos;
        private float _PosShiftX;

        private Vector3 _RunningLinePos;

        private Vector3 _FirstNumScale, _FirstNumShiftY, _CritShift;

#region Unity Functions
        private void Awake ()
        {
            GetComponentInParent<IDamageable>().OnDamage += DamageEvent;
            _LineObjects = new List<Transform>();

            _FirstNumScale = new Vector3 (1.2f, 1.2f, 1f);
            _FirstNumShiftY = new Vector3 (0f, 0.03f, 0f);
            _CritShift = new Vector3 (0.1f, 0.24f, 0f);
        }

        [ContextMenu("Generate 123456789")]
        public void Demo1234567890 ()
        {
            Debug.Log("Demoing 1234567890");

            _LineObjects.Add(Instantiate(_DamageLinePrefab, transform, false).transform);

            int num = 1234567890;

            if (Mathf.Floor(Mathf.Log10(num) + 1) % 2 == 1) _RunningPos = new Vector3(0f, 0.015f, 0f);
            else _RunningPos = new Vector3(0f, -0.015f, 0f);


            while (num != 0)
            {
                (_NextNum, _PosShiftX) = _DamgeSkin.GetNumber(num % 10, false);
                _NextNumTransform = Instantiate(_NextNum, _LineObjects[^1]).transform;
                _NextNumTransform.position = _RunningPos;

                _RunningPos.x -= _PosShiftX - 0.08f;
                _RunningPos.y *= -1;
                _RunningPos.z += 0.01f;
                num = num / 10;
            }

            _NextNumTransform.localScale = _FirstNumScale;
            _NextNumTransform.position += _FirstNumShiftY;
  
            Debug.Log(_LineObjects[^1].position);

            _RunningPos.x *= -0.5f;
            _LineObjects[^1].position += _RunningPos;

            Invoke("Remove", TTL);
        }

        private void Remove ()
        {
            DestroyImmediate(_LineObjects[^1]);
            _LineObjects.RemoveAt(_LineObjects.Count - 1);
        }

#endregion
#region Private Functions
        private void DamageEvent (object sender, OnDamageArgs e)
        {
            if (!_IsActive) return; 

            _RunningLinePos = Vector3.zero;
            _LinesWaiting = e.DamageLines;
            for (int i = 0; i < _LinesWaiting.Size; i++)
                Invoke("AddWaitingLine", LINE_DELAY * i); 
        }


        private void AddWaitingLine ()
        {
            if (_LinesWaiting.Size <= 0) return;

            AddDamageLine(_LinesWaiting[0].Damage, _LinesWaiting[0].IsCrit);
            _LinesWaiting.RemoveAt(0);
        }

        private void AddDamageLine (int num, bool isCrit)
        {
            _LineObjects.Add(Instantiate(_DamageLinePrefab, transform).transform);

            if (Mathf.Floor(Mathf.Log10(num) + 1) % 2 == 1) _RunningPos = new Vector3(0f, 0.015f, 0f);
            else _RunningPos = new Vector3(0f, -0.015f, 0f);

            while (num != 0)
            {
                (_NextNum, _PosShiftX) = _DamgeSkin.GetNumber(num % 10, isCrit);
                _NextNumTransform = Instantiate(_NextNum, _LineObjects[^1]).transform;
                _NextNumTransform.localPosition = _RunningPos;

                _RunningPos.x -= _PosShiftX - NUM_SPACE;
                _RunningPos.y *= -1;
                _RunningPos.z += 0.01f;
                num = num / 10;
            }

            _NextNumTransform.localScale = _FirstNumScale;
            _NextNumTransform.localPosition += _FirstNumShiftY;
  
            // Add Crit Effect
            if (isCrit)
            {
                (_NextNum, _PosShiftX) = _DamgeSkin.GetCritEffect();
                _NextNumTransform = Instantiate(_NextNum, _LineObjects[^1]).transform;
                _NextNumTransform.localPosition = _RunningPos;
                _NextNumTransform.localPosition += _CritShift;
            }

            _RunningPos.x *= -0.5f;
            _LineObjects[^1].localPosition = _RunningLinePos;
            _LineObjects[^1].localPosition += _RunningPos;

            _RunningLinePos.y += LINE_SPACE;
            _RunningLinePos.z -= 0.01f;


            Invoke("RemoveDamageLine", TTL);
        }

        private void RemoveDamageLine ()
        {
            Destroy(_LineObjects[0].gameObject);
            _LineObjects.RemoveAt(0);
        }
#endregion
    }
}