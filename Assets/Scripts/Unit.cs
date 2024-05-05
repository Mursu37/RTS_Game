using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour, IDamageable, IPriority
{
    public int Priority { get; set; } = 5;
    public float MaxHealth { get; set; } = 75f;
    public float CurrentHealth { get; set; }
    public float damage = 0f;
    public float attackRange = 1f;
    public float attackSpeed = 1f; // attacks per second
    public float movementSpeed = 2f;
    public float stopAttackDistance = 1.2f; // t‰m‰n pit‰‰ olla v‰h‰n isompi kuin attack range

    public float attackCooldown;

    public HealthTracker healthTracker;
    
    public GameObject bullet;
    public GameObject barrel;

    protected NavMeshAgent agent;
    

    void Start()
    {
        movementSpeed = 6.5f;
        attackCooldown = 1f / attackSpeed;
        CurrentHealth = MaxHealth;
        UpdateHealthUI();
        agent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgent();
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
    }

   

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        if (UnitSelectionManager.Instance.unitsSelected.Contains(gameObject))
        {
            UnitSelectionManager.Instance.unitsSelected.Remove(gameObject);
        }
    }
    protected void SetupNavMeshAgent()
    {
        agent.acceleration = 99999;
        agent.angularSpeed = 200;
        agent.speed = movementSpeed;
        agent.stoppingDistance = 0.5f;
    }


  
    
    public void Damage(float amount)
    {
        CurrentHealth -= amount;
        UpdateHealthUI();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(CurrentHealth, MaxHealth);
    }
    
    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        UpdateHealthUI();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
