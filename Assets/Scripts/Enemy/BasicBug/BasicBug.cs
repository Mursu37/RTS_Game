using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BasicBug
{
    public class BasicBug : Enemy, IPriority
    {
        public int Priority { get; set; } = 2;
        private StateController StateController { get; set; }
        
        public Collider target;
        public int currentTargetPriorityValue;
        
        public float aggroRange;
        public float attackRange;
        public float attackSpeed;
        public float attackDamage;

        public Animator animator;

        //public NavMeshAgent agent;
        
        public Collider FindHighestPriority(Collider[] targets)
        {
            Collider highestPriorityTarget = targets[0];
            var currentHighestPriorityValue = 1;

            foreach (var collider in targets)
            {
                var priority = collider.GetComponent<IPriority>();
                if (priority != null)
                {
                    if (priority.Priority > currentHighestPriorityValue)
                    {
                        highestPriorityTarget = collider;
                        currentHighestPriorityValue = priority.Priority;
                    }
                }
            }

            return currentHighestPriorityValue > currentTargetPriorityValue ? highestPriorityTarget : target;
        }

        public bool FindHigherPriorityTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange, ~LayerMask.GetMask("Attackble"));
            if (colliders.Length > 0)
            {
                Collider highestPriorityTarget = FindHighestPriority(colliders);
                if (highestPriorityTarget != target)
                {
                    target = highestPriorityTarget;
                    currentTargetPriorityValue = target.GetComponent<IPriority>().Priority;
                    return true;
                }
            }

            return false;
        }

        private void Awake()
        {
            MaxHealth = 75f;
            aggroRange = 5f;
            attackRange = 1.25f;
            attackSpeed = 0.625f;
            attackDamage = 5f; // 4
            
            movementSpeed = 5f;
            currentTargetPriorityValue = 1;
            
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