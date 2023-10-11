using UnityEngine;
using CoreSystems;

namespace MesoSystem
{
    public class MesoBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private const float TTL = 7f;
        private int _Meso;

#region Public Functions
        public void Init (int amount)
        {
            _Meso = Mathf.Abs(amount);
            animator.SetFloat("meso", _Meso);

            Invoke("CollectMeso", TTL);
        }

        public void CollectMeso ()
        {
            MesoManager.Instance?.AddMeso(_Meso);
            Destroy(gameObject);
        }
#endregion
    }    
}