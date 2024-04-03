using UnityEngine;

namespace Enemy.BasicBug
{
    public class AttackState : BasicBugState
    {
        public AttackState(StateController stateController, BasicBug enemy) : base(stateController, enemy)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Attacking");
            Enemy.agent.SetDestination(Enemy.transform.position);
        }
    }
}