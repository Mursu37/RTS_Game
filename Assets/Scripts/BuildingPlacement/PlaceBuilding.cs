using UnityEngine;
using UnityEngine.AI;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject turret;
    public GameObject hq;
    public GameObject barrack;

    public GameObject buildSite;
    private Camera _mainCamera;

    private bool _placingBuilding = false;
    private GameObject _beingPlaced;
    private GameObject _newBuilding;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void place_building()
    {
        // Look for objects at position not on Ground layer
        Collider[] collisions = Physics.OverlapBox(_newBuilding.transform.position,
            _newBuilding.GetComponentInChildren<BoxCollider>().bounds.extents + new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity,
            ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);

        // Collision box always collides with newly created object so we check if more than one collision occured
        if (collisions.Length > 1)
        {
            return;
        }

        GameObject building = Instantiate(buildSite, _newBuilding.transform.position, Quaternion.identity);
        var buildable = building.GetComponentInChildren<Buildable>();
        buildable.TimeToBuild = 5f;
        buildable.Building = _beingPlaced;
        building.transform.GetChild(0).transform.localScale = _newBuilding.transform.GetChild(0).transform.localScale;
        building.transform.GetChild(0).transform.position = _newBuilding.transform.GetChild(0).transform.position;
        Destroy(_newBuilding);
        _placingBuilding = false;   
    }

    private void set_building(GameObject building)
    {
        if (_placingBuilding)
        {
            Destroy(_newBuilding);
        }
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
            set_building(barrack);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            set_building(turret);
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
