using System.Collections;
using UnityEngine;

namespace Buildings.Turret
{
    public class Turret : MonoBehaviour, IDamageable
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        private Collider _target;
        private float _attackDamage;
        private float _attackSpeed;
        private float _attackRange;

        private bool _searchingForTarget;
        private bool _turretOn;
        
        [SerializeField] private GameObject _bullet;
        
        
        private void Awake()
        {
            MaxHealth = 7500f;
            CurrentHealth = MaxHealth;

            _attackDamage = 100f;
            _attackSpeed = 0.25f;
            _attackRange = 5f;

            _searchingForTarget = true;
            _turretOn = true;

            StartCoroutine(SearchAndDestroy());
        }

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        IEnumerator SearchAndDestroy()
        {
            while (_turretOn)
            {
                yield return StartCoroutine(Search());
                yield return StartCoroutine(Attack());
            }
        }

        IEnumerator Search()
        {
            while (_searchingForTarget)
            {
                yield return new WaitForSeconds(0.25f);
                Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRange - 1,
                    LayerMask.GetMask("Attackble"), QueryTriggerInteraction.Collide);

                if (colliders.Length > 0)
                {
                    _target = colliders[0];
                    _searchingForTarget = false;
                }
            }
        }

        IEnumerator Attack()
        {
            IDamageable damageable = _target.GetComponent<IDamageable>();
            if (damageable == null)
            {
                _searchingForTarget = true;
                yield break;
            }
            
            while (!_searchingForTarget)
            {
                if (_target == null)
                {
                    _searchingForTarget = true;
                    yield break;
                }
                
                if ((_target.transform.position - transform.position).magnitude < _attackRange)
                {
                    damageable.Damage(_attackDamage);
                    var bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
                    bullet.GetComponent<Bullet>().target = _target;
                    yield return new WaitForSeconds(_attackSpeed);
                }
                else
                {
                    _searchingForTarget = true;
                }
            }
        }
    }
}
