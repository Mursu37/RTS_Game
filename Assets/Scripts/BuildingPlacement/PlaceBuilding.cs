using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlaceBuilding : MonoBehaviour
{
    public bool mouseOverUI = false;
    
    public GameObject turret;
    public GameObject supplyDepo;
    public GameObject barrack;

    public GameObject buildSite;
    private Camera _mainCamera;

    private bool _placingBuilding = false;
    private GameObject _beingPlaced;
    private GameObject _newBuilding;
    private int _price;

    private ResourceManager _resourceManager;
    public static PlaceBuilding Instance { get; set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
    }

    private void place_building()
    {
        if (mouseOverUI) return;
        
        _newBuilding.GetComponentInChildren<Collider>().enabled = true;
        // Look for objects at position not on Ground layer
        var collisions = Physics.OverlapBox(_newBuilding.transform.position,
            _newBuilding.GetComponentInChildren<Collider>().bounds.extents + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity,
            ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);

        // Spaghetti but i just couldnt figure out how to do this any smarter with the way units are made
        var curatedCollisions = new List<Collider>();

        foreach (var collider in collisions)
        {
            if (collider.GetComponent<Unit>() == null)
            {
                curatedCollisions.Add(collider);
                continue;
            }
            if ((collider.bounds.center - _newBuilding.transform.position).magnitude <= 2f)
            {
                curatedCollisions.Add(collider);
            }
        }
        
        // Collision box always collides with newly created object so we check if more than one collision occured
        if (curatedCollisions.Count > 1)
        {
            _newBuilding.GetComponentInChildren<Collider>().enabled = false;
            return;
        }

        _resourceManager.SpendResource(Resource.Titanium , _price);
        GameObject building = Instantiate(buildSite, _newBuilding.transform.position, Quaternion.identity);
        var buildable = building.GetComponentInChildren<Buildable>();
        buildable.TimeToBuild = 5f;
        buildable.Building = _beingPlaced;
        building.transform.GetChild(0).transform.localScale =
            _newBuilding.GetComponentInChildren<Collider>().bounds.extents * 1.25f; // transform.GetChild(0).transform.localScale;
        building.transform.GetChild(0).transform.position = _newBuilding.transform.GetChild(0).transform.position;
        Destroy(_newBuilding);
        _placingBuilding = false;   
    }

    public void set_building(GameObject building, int price)
    {
        if (_placingBuilding)
        {
            Destroy(_newBuilding);
        }

        _price = price;
        _newBuilding = Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity);
        var collider = _newBuilding.GetComponentInChildren<Collider>();
        if (collider == null) _newBuilding.GetComponent<Collider>();
        collider.enabled = false;
        _newBuilding.GetComponentInChildren<NavMeshObstacle>().enabled = false;
        _newBuilding.GetComponentInChildren<Buildings.Building>().enabled = false;
        _newBuilding.GetComponentInChildren<Collider>().tag = "Untagged";
        _beingPlaced = building;
        _placingBuilding = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_resourceManager.CanAfford(30))
            {
                set_building(barrack, 30);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_resourceManager.CanAfford(50))
            {
                set_building(turret, 50);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_resourceManager.CanAfford(20))
            {
                set_building(supplyDepo, 20);
            }
        }

        if (_placingBuilding)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity,
                    LayerMask.GetMask("Ground")))
            {
                _newBuilding.transform.position = hit.point;
                if (Input.GetMouseButtonDown(0))
                {
                    place_building();
                }
            }
        }

        if (_placingBuilding && Input.GetMouseButtonDown(1))
        {
            Destroy(_newBuilding);
            _placingBuilding = false;
        }

    }
    
}
