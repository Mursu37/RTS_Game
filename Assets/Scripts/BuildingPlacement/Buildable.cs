using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Unity.VisualScripting;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    public GameObject Building { get; set; }
    public float TimeToBuild { get; set; }
    
    private Collider _currentWorker;

    private void Awake()
    {
        Building = null;
    }

    private void FixedUpdate()
    {
        if (Building != null)
        {
            if (TimeToBuild < 0)
            {
                Instantiate(Building, transform.parent.gameObject.transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }

            // check if worker is in building distance
            if (_currentWorker != null && 
                Vector3.Distance(transform.position, _currentWorker.transform.position) <=
                (transform.localScale / 2 + new Vector3(1, 1, 1)).magnitude) 
            {
                TimeToBuild -= Time.fixedDeltaTime;
            }
            else
            {
                if (_currentWorker != null)
                {
                    _currentWorker.GetComponent<UnitMovement>().DisableAsWorker();
                    _currentWorker = null;
                }
                Collider[] colliders = Physics.OverlapBox(transform.position,
                    (transform.localScale / 2) + new Vector3(1, 1, 1), Quaternion.identity,
                    ~LayerMask.GetMask("Ground"));
                foreach (var collision in colliders)
                {
                    if (collision.CompareTag("Worker"))
                    {
                        var worker = collision.GetComponent<UnitMovement>();
                        if (!worker.IsWorking())
                        {
                            _currentWorker = collision;
                            worker.SetAsWorker();
                            break;
                        }
                    }
                }
            }
            if (_currentWorker != null) TimeToBuild -= Time.fixedDeltaTime;
        }
    }

    private void OnDestroy()
    {
        if (_currentWorker != null) _currentWorker.GetComponent<UnitMovement>().DisableAsWorker();
    }
}
