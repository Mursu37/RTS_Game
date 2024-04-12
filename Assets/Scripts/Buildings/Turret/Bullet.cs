using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 10f;

    private Vector3 _targetPosition;
    private Vector3 _targetDirection;
    
    public Collider target;
    public float _damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other == target)
        {
            Destroy(gameObject);
        }
    }

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
    }
}
