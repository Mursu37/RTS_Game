using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    private Animator animator;
    public LayerMask ground;
    AttackController attackController;


    public bool isCommandedToMove;

    // TODO Move Gathering and Repairing to state machine
    // Gathering
    private bool _gathering;
    private bool _gatherOnCooldown;
    private bool _returningResources;
    private int _resourceCount;
    private int _resourceLimit;
    private Resource _resourceType;
    private IGatherable _resourceNode;
    private Collider _resourceNodeCollider;

    private Collider _hqCollider;
    private Vector3 _hq;
    private Vector3 _resourceLocation;
    [SerializeField] private TMP_Text resourceText;
    
    // Repairing
    private bool _repairing;
    private IDamageable _building;
    private float _repairSpeed;
    private int _repairedAmount;

    /// <summary>
    /// Returns short directional vector towards target position
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private Vector3 GetTargetDirection(Vector3 targetPosition)
    {
        return (targetPosition - transform.position).normalized * 1f;
    }
    
    private Vector3 GetClosestDepositionLocation()
    {
        float closestPositionDistance;
        Vector3 currentPosition = transform.position;
        Vector3 closestPosition = _hqCollider.ClosestPoint(currentPosition);

        GameObject[] supplyDepos = GameObject.FindGameObjectsWithTag("SupplyDepo");
        if (supplyDepos.Length == 0) return closestPosition;

        closestPositionDistance = (_hqCollider.ClosestPoint(currentPosition) - currentPosition).magnitude;
        foreach (var depo in supplyDepos)
        {
            Vector3 depoClosestPoint = depo.GetComponent<Collider>().ClosestPoint(currentPosition);
            float depoDistance = (depoClosestPoint - currentPosition).magnitude;
            if (depoDistance < closestPositionDistance)
            {
                closestPosition = depoClosestPoint;
                closestPositionDistance = depoDistance;
            }
        }


        return closestPosition;
    }


    private void Awake()
    {
        _gathering = false;
        _gatherOnCooldown = false;
        _resourceLimit = 10;

        _repairing = false;
        _repairedAmount = 0;
        // repair 2.5 times a second. repairs one health at a time
        _repairSpeed = 1 / 2.5f;
        _hqCollider = GameObject.FindWithTag("HQ").GetComponentInChildren<Collider>();
        _hq = _hqCollider.transform.position;
    }

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
    }

    IEnumerator Repair()
    {
        yield return new WaitForFixedUpdate();
        while (_repairing)
        {
            yield return new WaitForSeconds(_repairSpeed);
            if (agent.remainingDistance <= 0.5f)
            {
                if (!ResourceManager.Instance.CanAfford(1) || _building.CurrentHealth >= _building.MaxHealth) break; 
                _building.Heal(1f);
                _repairedAmount++;
                if (_repairedAmount > 10)
                {
                    ResourceManager.Instance.SpendResource(Resource.Titanium ,1);
                    _repairedAmount = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    IEnumerator Gather()
    {
        yield return new WaitForFixedUpdate();
        while (_gathering)
        {
            if (_resourceCount >= _resourceLimit || _resourceNodeCollider == null)
            {
                Vector3 depositionLocation = GetClosestDepositionLocation();
                agent.SetDestination(depositionLocation - GetTargetDirection(depositionLocation));
                yield return new WaitForSeconds(0.1f);
                Debug.Log(agent.remainingDistance);
                Debug.Log(depositionLocation);
                
                if (agent.remainingDistance <= 0.5f)
                {
                    var resourceManager = ResourceManager.Instance;
                    resourceManager.AddResource(_resourceType ,_resourceCount);
                    _resourceCount = 0;
                    resourceText.text = _resourceCount + " / " + _resourceLimit;
                    
                    if (_resourceNodeCollider == null)
                    {
                        _gathering = false;
                        break;
                    }
                    
                    agent.SetDestination(_resourceNodeCollider.ClosestPoint(transform.position));
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
                if (agent.remainingDistance > 0.5f)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }
                if (!_gatherOnCooldown) _resourceCount += _resourceNode.Gather();
                _gatherOnCooldown = true;
                agent.SetDestination(agent.transform.position);
                resourceText.text = _resourceCount + " / " + _resourceLimit;
                yield return new WaitForSeconds(1f);
                _gatherOnCooldown = false;
            }
        }

        resourceText.text = "";
    }

    public bool IsWorking()
    {
        if (_repairing || _gathering)
        {
            return true;
        }
        return false;
    }
    
    private void Update()
    {
        // HAISTA VITTU NAVMESH / CARVE
        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
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
            
            if (this.CompareTag("Worker") && Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable")))
            {
                agent.stoppingDistance = 0.5f;
                isCommandedToMove = true;
                _gathering = false;
                _repairing = false;
                Vector3 destination;
                destination = hit.collider.ClosestPoint(transform.position);
                agent.SetDestination(destination);
                if (hit.transform.CompareTag("ResourceNode") && !_gathering)
                {
                    _resourceNode = hit.collider.GetComponent<IGatherable>();
                    _resourceNodeCollider = hit.collider;
                    if (_resourceNode.ResourceType != _resourceType) _resourceCount = 0;
                    
                    _resourceType = _resourceNode.ResourceType;
                    _gathering = true;
                    _resourceLocation = hit.transform.position;
                    StopAllCoroutines();
                    resourceText.text = "";
                    StartCoroutine(Gather());
                }
                
                else if (hit.collider.GetComponent<IBuilding>() != null && !_repairing)
                {
                    _building = hit.collider.GetComponent<IDamageable>();
                    _repairing = true;
                    StopAllCoroutines();
                    resourceText.text = "";
                    StartCoroutine(Repair());
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Attackble")))
            {
                agent.stoppingDistance = 0.5f;
                isCommandedToMove = false;

                //fix for infantry movement without having to rewrite unit selection script
                FixMovement infantryMovementFix = GetComponent<FixMovement>();
                if (infantryMovementFix != null) infantryMovementFix.isCommandedToMove = false;

                agent.SetDestination(hit.point - GetTargetDirection(hit.point));
                _gathering = false;
                _repairing = false;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                agent.stoppingDistance = 0.5f;
                isCommandedToMove = true;
                attackController.targetToAttack = null;

                //fix for infantry movement without having to rewrite unit selection script
                FixMovement infantryMovementFix = GetComponent<FixMovement>();
                if (infantryMovementFix != null) infantryMovementFix.isCommandedToMove = true;

                agent.SetDestination(hit.point - GetTargetDirection(hit.point));
                _gathering = false;
                _repairing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}
