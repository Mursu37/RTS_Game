
using System;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IDamageable 
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            Debug.Log(CurrentHealth);
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        
        public void Die()
        {
            Debug.Log("Dead");
            Destroy(gameObject);
        }

        private void Awake()
        {
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }
    }
}