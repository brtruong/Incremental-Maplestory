using System;
using UnityEngine;
using CoreSystems;

namespace MesoSystem
{
    public class MesoManager : MonoBehaviour
    {
        // Members
        public static MesoManager Instance {get; private set;}
        
        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        [SerializeField] private GameObject _MesoObject;

        [SerializeField] private Meso _Meso;

        private bool returnOut;

        public event EventHandler<OnMesoUpdateArgs> OnMesoUpdate;

#region Unity Functions
        private void Awake ()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
        }
#endregion
#region Public Functions
        public void LoadMeso (GameData data)
        {
            _Logger.Log(gameObject, "Loading Meso");

            _Meso = data.Meso;
            OnMesoUpdate?.Invoke(this, new OnMesoUpdateArgs{Meso = _Meso.Amount});
        }

        public bool MinusMeso (int amount)
        {
            _Logger.Log(gameObject, "Subtracting {" + Mathf.Abs(amount) + "}");

            returnOut = _Meso.Subtract(amount);
            OnMesoUpdate?.Invoke(this, new OnMesoUpdateArgs{Meso = _Meso.Amount});
            return returnOut;
        } 

        public bool AddMeso (int amount)
        {
            _Logger.Log(gameObject, "Adding {" + Mathf.Abs(amount) + "}");

            returnOut = _Meso.Add(amount);
            OnMesoUpdate?.Invoke(this, new OnMesoUpdateArgs{Meso = _Meso.Amount});
            return returnOut;
        }

        public void SpawnMeso (Vector3 position, Quaternion rotation, int amount)
        {
            Instantiate(_MesoObject, position, rotation).GetComponent<MesoBehaviour>().Init(amount);
        }
#endregion
    }
}