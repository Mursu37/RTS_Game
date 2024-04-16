using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buildings.HQ
{
    public class HQ : MonoBehaviour, IDamageable
    {

        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        private void Awake()
        {
            MaxHealth = 500f;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
    }
}
