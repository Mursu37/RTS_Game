using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.BasicBug
{
    public class SearchState : BasicBugState
    {
        private Vector3 _hq;
        public SearchState(StateController stateController, BasicBug enemy) : base(stateController, enemy)
        {
            _hq = GameObject.FindWithTag("HQ").transform.position;
        }

        private void SearchForTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.aggroRange);
            if (colliders.Length > 0)
            {
                float currentShortest = Mathf.Infinity;
                foreach (var collider in colliders)
                {
                    var damageble = collider.GetComponent<IDamageable>();
                    if (!collider.CompareTag("Enemy") && damageble != null)
                    {
                        float currentDistance = (collider.transform.position - Enemy.transform.position).magnitude;
                        if (currentDistance < currentShortest)
                        {
                            Enemy.target = collider;
                            currentShortest = currentDistance;
                        }
                    }
                }

                if (Enemy.target == null) return;
                StateController.ChangeState(StateController.ChaseState);
            }
        }

        public override void OnEnter()
        {
            Enemy.agent.SetDestination(_hq);
            Enemy.animator.SetTrigger("Run");
        }
        public override void OnExit()
        {
            Enemy.animator.ResetTrigger("Run");
        }

        public override void OnFixedUpdate()
        {
            SearchForTarget();
        }
    }
}