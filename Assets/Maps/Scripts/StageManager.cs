using System;
using System.Collections.Generic;
using UnityEngine;
using AudioSystem;
using MonsterSystem;

namespace MapSystem
{
    public class StageManager : MonoBehaviour
    {
        // Members
        public static StageManager Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        [SerializeField] private GameObject _MonsterObject;
        [SerializeField] private Map[] _Maps;
        [SerializeField] private bool _Spawn;

        [Header("Stage Info")]
        [SerializeField] private Stage _Stage;
        
        private Map _LoadedMap;
        private Dictionary<GameObject, int> _MonsterTable;
        private List<int> _RespawnInLocation;
        private const float _RESPAWN_RATE = 7f;

        public event EventHandler<OnStageUpdateArgs> OnStageUpdate;

#region Unity Functions
        private void Awake ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);

            _MonsterTable = new Dictionary<GameObject, int>();
            _RespawnInLocation = new List<int>();
        }
#endregion
#region Public Functions
        public void LoadStage (Stage stage)
        {
            if (stage == null) return;
            
            _Logger.Log(gameObject, "Loading Stage Data");
            _Stage = stage;
            OnStageUpdate?.Invoke(this, new OnStageUpdateArgs {
                Level = _Stage.Level, KillCount = _Stage.KillCount, KillRequirement = _Stage.KillRequirement});
        }

        public void LoadMap (int idx)
        {
            if (_Maps[idx] == null) return;

            _Logger.Log(gameObject, "Loading " + _Maps[idx].Name);

            AudioManager.Instance.PlayAudio(_Maps[idx].BGM);
            _LoadedMap = _Maps[idx];
            
            OnStageUpdate?.Invoke(this, new OnStageUpdateArgs {
                Level = _Stage.Level, KillCount = _Stage.KillCount, KillRequirement = _Stage.KillRequirement});
        }

        [ContextMenu("Start Spawn")]
        public void StartSpawn ()
        {
            LoadMap(0);

            _Logger.Log(gameObject, "Star Monster Spawning");

            for (int i = 0; i < _LoadedMap.SpawnLocations.Length; i++)
                _RespawnInLocation.Add(i);
            
            InvokeRepeating("SpawnMonsters", 0f, _RESPAWN_RATE);
        }

        public void StopSpawn ()
        {
            CancelInvoke();

            foreach (Transform child in transform)
                Destroy(child.gameObject);
            
            _MonsterTable.Clear();
        }

        public void MonsterDied (GameObject monster)
        {
            if (!_MonsterTable.ContainsKey(monster)) return;

            _RespawnInLocation.Add(_MonsterTable[monster]);
            _MonsterTable.Remove(monster);
            _Stage.IncrementKillCount();

            OnStageUpdate?.Invoke(this, new OnStageUpdateArgs {
                Level = _Stage.Level, KillCount = _Stage.KillCount, KillRequirement = _Stage.KillRequirement});
        }
#endregion
#region Private Functions
        private void SpawnMonsters ()
        {
            _Logger.Log(gameObject, "Spawning Monsters");
            if (!_Spawn) return;

            foreach (int i in _RespawnInLocation)
            {
                GameObject obj = Instantiate(_MonsterObject, transform);
                obj.transform.SetSiblingIndex(i);
                obj.transform.position = _LoadedMap.SpawnLocations[i];
                MonsterBehaviour monsterBehaviour = obj.GetComponent<MonsterBehaviour>();
                monsterBehaviour.Init(_LoadedMap.SpwanableMobs[0], _Stage.Level);
                _MonsterTable.Add(obj, i);
            }

            _RespawnInLocation.Clear();
        }
#endregion
    }
}