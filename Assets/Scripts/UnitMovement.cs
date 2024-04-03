using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    public bool isCommandedToMove;

    private bool _gathering;
    private bool _returningResources;
    private int _resourceCount;
    private int _resourceLimit;
    private Vector3 _hq;
    private Vector3 _resourceLocation;

    private void Awake()
    {
        _gathering = false;
        _resourceLimit = 10;
        _hq = GameObject.FindWithTag("HQ").transform.position;
    }

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    IEnumerator Gather()
    {
        while (_gathering)
        {
            if (_resourceCount >= _resourceLimit)
            {
                agent.SetDestination(_hq);
                yield return new WaitForSeconds(0.1f);
                
                Debug.Log(agent.remainingDistance);
                if (agent.remainingDistance < 1.5f)
                {
                    _resourceCount = 0;
                    Debug.Log(_resourceCount);
                    agent.SetDestination(_resourceLocation);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                if (agent.remainingDistance > 1.5f)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                _resourceCount++;
                Debug.Log(_resourceCount);
                yield return new WaitForSeconds(1f);     
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                isCommandedToMove = true;
                agent.SetDestination(hit.point);
                _gathering = false;
                StopCoroutine(Gather());
            }

            if (this.CompareTag("Worker") && Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable")))
            {
                if (hit.transform.CompareTag("ResourceNode"))
                {
                    _gathering = true;
                    _resourceLocation = hit.point;
                    Debug.Log(_gathering);
                    StartCoroutine(Gather());
                }
            }
        }

        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (_gathering)
        {
            
        }
    }
}
