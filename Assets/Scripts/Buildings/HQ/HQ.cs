using UnityEngine;

namespace Buildings.HQ
{
    public class HQ : MonoBehaviour, IDamageable
    {

        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        private void Awake()
        {
            MaxHealth = 10000f;
            CurrentHealth = MaxHealth;
        }

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
