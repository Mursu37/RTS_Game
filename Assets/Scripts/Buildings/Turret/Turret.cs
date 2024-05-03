using System;
using System.Collections;
using UnityEngine;

namespace Buildings.Turret
{
    public class Turret : Building
    {
        private Collider _target;
        private float _attackDamage;
        private float _attackSpeed;
        private float _attackRange;

        private bool _searchingForTarget;
        private bool _turretOn;

        Animator _animator;
        
        [SerializeField] private GameObject _bullet;
        [SerializeField] private GameObject rightBarrel;
        [SerializeField] private GameObject leftBarrel;
        private bool _shootFromRight;

        [SerializeField] private GameObject turretHead;
        private Quaternion lastRotation;

        private Quaternion startingRotation;
        
        private void Awake()
        {
            MaxHealth = 4000f;

            _attackDamage = 2f;
            _attackSpeed = 0.3f;
            _attackRange = 5f;

            _searchingForTarget = true;
            _turretOn = true;
            _shootFromRight = true;
            _animator = GetComponent<Animator>();
            
            lastRotation = turretHead.transform.localRotation;
            startingRotation = turretHead.transform.localRotation;
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(SearchAndDestroy());
        }
        
        private void LateUpdate()
        {
            if (!_searchingForTarget && _target != null)
            {
                Vector3 lookDirection = _target.transform.position - turretHead.transform.position;
                lookDirection.Normalize();
                turretHead.transform.rotation = Quaternion.RotateTowards(lastRotation, Quaternion.LookRotation(lookDirection), 360f * Time.deltaTime);
                lastRotation = turretHead.transform.rotation;
            }
        }

        IEnumerator SearchAndDestroy()
        {
            while (_turretOn)
            {
                yield return StartCoroutine(Search());
                yield return StartCoroutine(Attack());
                _animator.SetBool("isAttacking", false);
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
            _animator.SetBool("isAttacking", true);
            IDamageable damageable = _target.GetComponent<IDamageable>();
            lastRotation = turretHead.transform.rotation;
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
                    GameObject bullet;
                    if (_shootFromRight)
                    {
                        bullet = Instantiate(_bullet, rightBarrel.transform.position, Quaternion.identity);
                        _shootFromRight = false;
                    }
                    else
                    {
                        bullet = Instantiate(_bullet, leftBarrel.transform.position, Quaternion.identity);
                        _shootFromRight = true;
                    }
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
