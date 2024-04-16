using System;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlaceBuilding : MonoBehaviour
{
    public bool mouseOverUI = false;
    
    public GameObject turret;
    public GameObject hq;
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
        Collider[] collisions = Physics.OverlapBox(_newBuilding.transform.position,
            _newBuilding.GetComponentInChildren<Collider>().bounds.extents + new Vector3(1, 1, 1), Quaternion.identity,
            ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);
        
        // Collision box always collides with newly created object so we check if more than one collision occured
        if (collisions.Length > 1)
        {
            _newBuilding.GetComponentInChildren<Collider>().enabled = false;
            return;
        }

        _resourceManager.SpendResource(Resource.Titanium , _price);
        GameObject building = Instantiate(buildSite, _newBuilding.transform.position, Quaternion.identity);
        var buildable = building.GetComponentInChildren<Buildable>();
        buildable.TimeToBuild = 5f;
        buildable.Building = _beingPlaced;
        building.transform.GetChild(0).transform.localScale = _newBuilding.transform.GetChild(0).transform.localScale;
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
        _newBuilding.GetComponentInChildren<Collider>().enabled = false;
        _newBuilding.GetComponentInChildren<NavMeshObstacle>().enabled = false;
        _beingPlaced = building;
        _placingBuilding = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_resourceManager.CanAfford(60))
            {
                set_building(barrack, 60);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_resourceManager.CanAfford(100))
            {
                set_building(turret, 100);
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
