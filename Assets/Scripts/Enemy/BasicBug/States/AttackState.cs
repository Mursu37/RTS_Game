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
            _attackTimer = Enemy.attackSpeed;
            Enemy.animator.SetTrigger("Attack");
        }

        public override void OnExit()
        {
            Enemy.animator.ResetTrigger("Attack");
        }

        private void Attack()
        {
            if (Enemy.target == null)
            {
                StateController.ChangeState(StateController.SearchState);
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.attackRange);

            _enemyInRange = false;
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider == Enemy.target)
                    {
                        var damageble = collider.GetComponent<IDamageable>();
                        damageble.Damage(Enemy.attackDamage);
                        _attackTimer = Enemy.attackSpeed;
                        _enemyInRange = true;
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
            _attackTimer -= Time.fixedDeltaTime;
            if (_attackTimer <= 0)
            {
                Attack();
            }
        }
    }
}