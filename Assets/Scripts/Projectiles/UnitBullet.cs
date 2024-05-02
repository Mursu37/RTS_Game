using System;
using UnityEngine;

namespace Projectiles
{
    public class UnitBullet : MonoBehaviour
    {
        private float _speed = 15f;

        private Vector3 _targetPosition;
        private Vector3 _targetDirection;
    
        public Collider target;
        public float _damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other == target)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        private void Update()
        {
            if (target == null) Destroy(transform.parent.gameObject);
            transform.parent.transform.LookAt(target.transform, Vector3.up);
            _targetDirection = (_targetPosition - transform.position).normalized;
            transform.position += _targetDirection * (_speed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (target == null) Destroy(transform.parent.gameObject);
            else
            {
                _targetPosition = target.transform.position;   
            }
        }
    }
}