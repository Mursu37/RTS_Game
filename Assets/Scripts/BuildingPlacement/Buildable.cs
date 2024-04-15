using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class Buildable : MonoBehaviour, IBuilding
{
    public GameObject Building { get; set; }
    public float TimeToBuild { get; set; }

    private int _workerCount;

    private void Awake()
    {
        Building = null;
    }

    private void FixedUpdate()
    {
        _workerCount = 0;
        if (Building != null)
        {
            if (TimeToBuild < 0)
            {
                Instantiate(Building, transform.parent.gameObject.transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }

            Collider[] colliders = Physics.OverlapBox(transform.position, (transform.localScale / 2) + new Vector3(1, 1, 1), Quaternion.identity,
                ~LayerMask.GetMask("Ground"));
            foreach (var collision in colliders)
            {
                if (collision.CompareTag("Worker"))
                {
                    _workerCount++;
                }
            }

            if (_workerCount > 0)
            {
                TimeToBuild -= Time.fixedDeltaTime;
            }
        }
    }

    public void BuildingSelected()
    {
        return;
    }

    public void BuildingUnselected()
    {
        return;
    }
}
