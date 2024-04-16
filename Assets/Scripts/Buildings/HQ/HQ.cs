using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buildings.HQ
{
    public class HQ : Building
    {
        private void Awake()
        {
            MaxHealth = 500f;
        }
        public override void Die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
        }
    }
}
