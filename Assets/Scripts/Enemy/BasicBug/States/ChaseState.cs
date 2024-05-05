using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BasicBug
{
    public class ChaseState : BasicBugState
    {
        public ChaseState(StateController stateController, BasicBug enemy) : base(stateController, enemy)
        {
        }

        private void CheckIfTargetInRange()
        {
            if (Enemy.target == null)
            {
                StateController.ChangeState(StateController.SearchState);
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.attackRange + 0.1f);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider == Enemy.target)
                    {
                        StateController.ChangeState(StateController.AttackState);
                    }
                }
            }
        }

        public override void OnEnter()
        {
            Enemy.agent.SetDestination(Enemy.target.transform.position);
            Enemy.animator.SetTrigger("Run");
        }
        public override void OnExit()
        {
            if (Enemy.target == null) Enemy.currentTargetPriorityValue = 0;
            Enemy.animator.ResetTrigger("Run");
        }

        public override void OnFixedUpdate()
        {
            Enemy.FindHigherPriorityTarget();
            CheckIfTargetInRange();
            Enemy.agent.SetDestination(Enemy.target.ClosestPoint(Enemy.transform.position));
        }
    }
}