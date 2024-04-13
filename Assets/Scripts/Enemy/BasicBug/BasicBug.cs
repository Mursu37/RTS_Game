using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BasicBug
{
    public class BasicBug : Enemy
    {
        private StateController StateController { get; set; }
        public Collider target;
        public float aggroRange;
        public float attackRange;
        public float attackSpeed;
        public float attackDamage;

        public Animator animator;

        //public NavMeshAgent agent;

        private void Awake()
        {
            MaxHealth = 2000f;
            aggroRange = 5f;
            attackRange = 1f;
            attackSpeed = 0.5f;
            attackDamage = 10f;
            
            movementSpeed = 5f;
            
            agent = GetComponent<NavMeshAgent>();

            animator = GetComponent<Animator>();
            
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