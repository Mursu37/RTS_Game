using UnityEngine;

namespace Enemy.BasicBug
{
    public class AttackState : BasicBugState
    {
        private float _attackTimer;
        private bool _enemyInRange;
        public AttackState(StateController stateController, BasicBug enemy) : base(stateController, enemy)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Attacking");
            Enemy.transform.LookAt(Enemy.target.transform);
            _attackTimer = Enemy.attackSpeed;
            Enemy.animator.SetTrigger("Attack");
        }

        public override void OnExit()
        {
            if (Enemy.target == null) Enemy.currentTargetPriorityValue = 0;
            Enemy.animator.ResetTrigger("Attack");
        }

        private void Attack()
        {
            if (Enemy.target == null)
            {
                StateController.ChangeState(StateController.SearchState);
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.attackRange + 0.1f);

            _enemyInRange = false;
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider == Enemy.target)
                    {
                        // TODO Make turning smooth
                        Enemy.transform.LookAt(Enemy.target.transform);
                        var damageble = collider.GetComponent<IDamageable>();
                        if (damageble != null)
                        {
                            damageble.Damage(Enemy.attackDamage);
                            _attackTimer = Enemy.attackSpeed;
                            _enemyInRange = true;
                        }
                    }
                }
            }
            if (!_enemyInRange)
            {
                Enemy.target = null;
                StateController.ChangeState(StateController.SearchState);
                return;
            }
        }

        public override void OnFixedUpdate()
        {
            if (Enemy.FindHigherPriorityTarget()) StateController.ChangeState(StateController.ChaseState);
            _attackTimer -= Time.fixedDeltaTime;
            if (_attackTimer <= 0)
            {
                Attack();
            }
        }
    }
}