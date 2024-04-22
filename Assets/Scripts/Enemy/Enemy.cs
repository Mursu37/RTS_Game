
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IDamageable 
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public NavMeshAgent agent;
        public float movementSpeed;
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
        
        protected virtual void SetupNavMeshAgent()
        {
            agent.acceleration = 99999;
            agent.angularSpeed = 200;
            agent.speed = movementSpeed;
            agent.stoppingDistance = 0.5f;
        }
        
        protected virtual void UpdateHealthUI()
        {
            healthTracker.UpdateSliderValue(CurrentHealth, MaxHealth);
        }
        
        public void Die()
        {
            Destroy(transform.parent.gameObject);
        }

        private void Awake()
        {
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
            SetupNavMeshAgent();
        }
    }
}