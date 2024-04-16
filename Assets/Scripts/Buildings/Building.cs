using System;
using UnityEngine;

namespace Buildings
{
    public abstract class Building : MonoBehaviour, IDamageable, IBuilding
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        [SerializeField] protected GameObject indicator;

        protected virtual void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0)
            {
                Die();
            }
        }
        
        public virtual void Heal(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public virtual void Die()
        {
            BuildingUnselected();
            Destroy(gameObject);
        }

        public virtual void BuildingSelected()
        {
            return;
        }

        public virtual void BuildingUnselected()
        {
            return;
        }
    }
}