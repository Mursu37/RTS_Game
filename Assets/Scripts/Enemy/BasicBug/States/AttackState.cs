using UnityEngine;

namespace Enemy.BasicBug
{
    public class AttackState : BasicBugState
    {
        private float _attackTimer;
        public AttackState(StateController stateController, BasicBug enemy) : base(stateController, enemy)
        {
        }

        public override void OnEnter()
        {
            _attackTimer = Enemy.attackSpeed;
        }

        private void Attack()
        {
            if (Enemy.target == null)
            {
                StateController.ChangeState(StateController.SearchState);
                Enemy.target = null;
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.attackRange);
            
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider == Enemy.target)
                    {
                        var damageble = collider.GetComponent<IDamageable>();
                        damageble.Damage(Enemy.attackDamage);
                        _attackTimer = Enemy.attackSpeed;
                    }
                }
            }
            else
            {
                StateController.ChangeState(StateController.SearchState);
                Enemy.target = null;
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