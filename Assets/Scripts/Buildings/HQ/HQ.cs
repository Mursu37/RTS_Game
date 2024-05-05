using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buildings.HQ
{
    public class HQ : Building
    {
        public GameObject EndScreen;
        private void Awake()
        {
            MaxHealth = 15f;
        }
        public override void Die()
        {
           
            Destroy(gameObject);
            EndScreen.SetActive(true);
        }
    }
}
