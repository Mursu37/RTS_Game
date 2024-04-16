using System;
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
    private Resource _resourceType;
    private IGatherable _resourceNode;
    private Collider _resourceNodeCollider;
    
    private Vector3 _hq;
    private Vector3 _resourceLocation;

    /// <summary>
    /// Returns short directional vector towards target position
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private Vector3 GetTargetDirection(Vector3 targetPosition)
    {
        return (targetPosition - transform.position).normalized * 0.1f;
    }

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
            if (_resourceCount >= _resourceLimit || _resourceNodeCollider == null)
            {
                agent.SetDestination(_hq - GetTargetDirection(_hq));
                yield return new WaitForSeconds(0.1f);
                
                if (agent.remainingDistance <= 0.5f)
                {
                    var resourceManager = ResourceManager.Instance;
                    resourceManager.AddResource(_resourceType ,_resourceCount);
                    _resourceCount = 0;
                    
                    if (_resourceNodeCollider == null)
                    {
                        _gathering = false;
                        StopCoroutine(Gather());
                        break;
                    }
                    
                    agent.SetDestination(_resourceLocation - GetTargetDirection(_resourceLocation));
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                if (agent.remainingDistance > 0.5f)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                _resourceCount += _resourceNode.Gather();
                yield return new WaitForSeconds(1f);
                agent.SetDestination(agent.transform.position);
            }
        }
    }
    
    private void Update()
    {
        // HAISTA VITTU NAVMESH / CARVE
        if (agent.hasPath == false ||agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
        
        if (_gathering)
        {
            if (_resourceNode == null)
            {
                _gathering = false;
                StopCoroutine(Gather());
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
           
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                isCommandedToMove = true;
                agent.SetDestination(hit.point - GetTargetDirection(hit.point));
                if (_gathering)
                {
                    _gathering = false;
                    StopCoroutine(Gather());
                }
            }

            
            if (this.CompareTag("Worker") && Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable")))
            {
                if (hit.transform.CompareTag("ResourceNode"))
                {
                    _resourceNode = hit.collider.GetComponent<IGatherable>();
                    _resourceNodeCollider = hit.collider;
                    if (_resourceNode.ResourceType != _resourceType) _resourceCount = 0;
                    
                    _resourceType = _resourceNode.ResourceType;
                    _gathering = true;
                    _resourceLocation = hit.transform.position;
                    StartCoroutine(Gather());
                }  
                agent.SetDestination(hit.transform.position - GetTargetDirection(hit.transform.position));
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}
