using System;
using UnityEngine;

namespace Buildings.ProductionBuilding
{
    public class ProductionBuilding : MonoBehaviour, IDamageable
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        private void Awake()
        {
            MaxHealth = 5000f;
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
