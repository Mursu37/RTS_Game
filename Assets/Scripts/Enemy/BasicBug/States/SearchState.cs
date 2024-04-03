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

        public override void OnEnter()
        {
            Enemy.agent.SetDestination(_hq);
        }

        public override void OnFixedUpdate()
        {
            if (Enemy.agent.remainingDistance <= 0)
            {
                StateController.ChangeState(StateController.AttackState);
            }
        }
    }
}