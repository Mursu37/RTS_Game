
using System.Diagnostics;
using UnityEngine.AI;

namespace Enemy.BasicBug
{
    public class BasicBug : Enemy
    {
        private StateController StateController { get; set; }

        public NavMeshAgent agent;

        private void Awake()
        {
            
            MaxHealth = 2000f;

            agent = GetComponent<NavMeshAgent>();
            StateController = new StateController(this);
            StateController.Initialize();
        }

        private void Update()
        {
            StateController.Update();
        }

        private void FixedUpdate()
        {
            StateController.FixedUpdate();
        }
    }
}