using UnityEngine;
using UnityEngine.UI;
using DamageSystem;

namespace UISystem
{
    public class HealthBarUI : MonoBehaviour
    {
        // Members
        [SerializeField] private Canvas _HealthBar;
        [SerializeField] private Slider _HealthBarFill;

        private IDamageable _Damageable;
        
#region Unity Functions
        private void Start ()
        {
            _HealthBar.enabled = false;

            _Damageable = GetComponentInParent<IDamageable>();
            _Damageable.OnDamage += UpdateHealthBar;
        }
#endregion
#region Public Functions
        public void UpdateHealthBar (object sender, OnDamageArgs e)
        {
            if (e.HP <= e.MaxHP) _HealthBar.enabled = true;
            if (e.IsDead)
            {
                _HealthBar.enabled = false;
                _Damageable.OnDamage -= UpdateHealthBar;
            }
            
            _HealthBarFill.value = (float)e.HP / e.MaxHP;
        }
#endregion
    }
}
