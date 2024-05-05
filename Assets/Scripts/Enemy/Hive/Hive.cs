using System.Collections;
using System.Collections.Generic;
using Enemy.Spawning;
using UnityEngine;

namespace Enemy.Hive
{
    public class Hive : MonoBehaviour, IDamageable, IPriority
    {
        public int Priority { get; set; } = 1;
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public HealthTracker healthTracker;
        [SerializeField] private GameObject basicBug;
        private bool _halfHealthNotReached = true;

        private EnemySpawning _enemySpawningManager;
        
        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            
            if (CurrentHealth <= MaxHealth / 2 && _halfHealthNotReached)
            {
                _halfHealthNotReached = false;
                StartCoroutine(SpawnEnemies(5));
            }
            
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
            _enemySpawningManager.spawnLocations.Remove(transform.parent.gameObject);
            _enemySpawningManager.HiveDestroyed();
            Destroy(transform.parent.gameObject);
        }

        IEnumerator SpawnEnemies(int enemyCount)
        {
            float spawnRate = 0.5f / enemyCount; // spawn all enemies within 0.5 seconds
            for (int i = 0; i < 5; i++)
            {
                Instantiate(basicBug, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnRate);
            }
        }
    
        private void Awake()
        {
            MaxHealth = 750f;
        }
    
        private void Start()
        {
            CurrentHealth = MaxHealth;
            _enemySpawningManager = EnemySpawning.Instance;
        }
    }
}

