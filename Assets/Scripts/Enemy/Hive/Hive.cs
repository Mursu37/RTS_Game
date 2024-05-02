using UnityEngine;

namespace Enemy.Hive
{
    public class Hive : MonoBehaviour, IDamageable
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public HealthTracker healthTracker;
        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth <= 0)
            {
                Die();
            }
            UpdateHealthUI();
        }
            
        public void Heal(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
            
        private void UpdateHealthUI()
        {
            healthTracker.UpdateSliderValue(CurrentHealth, MaxHealth);
        }
            
        public void Die()
        {
            Destroy(transform.parent.gameObject);
        }
    
        private void Awake()
        {
            MaxHealth = 750f;
        }
    
        private void Start()
        {
            CurrentHealth = MaxHealth;
        }
    }
}

