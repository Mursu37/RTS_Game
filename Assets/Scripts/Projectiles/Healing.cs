using System;
using UnityEngine;

namespace Projectiles
{
    public class Healing : MonoBehaviour
    {
        private float _speed = 7.5f;

        private Vector3 _targetPosition;
        private Vector3 _targetDirection;
    
        public Collider target;
        public float healAmount;

        private void Update()
        {
            _targetDirection = (_targetPosition - transform.position).normalized;
            transform.position += _targetDirection * (_speed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (target == null) Destroy(gameObject);
            else
            {
                _targetPosition = target.transform.position;   
            }
            
            if ((target.transform.position - transform.position).magnitude < 0.1f)
            {
                target.GetComponent<IDamageable>().Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
