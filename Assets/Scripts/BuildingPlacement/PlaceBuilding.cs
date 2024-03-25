using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject turret;
    public GameObject hq;
    public GameObject barrack;
    private Camera _mainCamera;

    private bool _placingBuilding = false;
    private GameObject _newBuilding;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void place_building()
    {
        // Look for objects at position not on Ground layer
        Collider[] collisions = Physics.OverlapBox(_newBuilding.transform.position,
            _newBuilding.GetComponentInChildren<BoxCollider>().bounds.extents + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity,
            ~LayerMask.GetMask("Ground"));

        // Collision box always collides with newly created object so we check if more than one collision occured
        if (collisions.Length > 1)
        {
            return;
        }
        
        _placingBuilding = false;   
    }

    private void set_building(GameObject building)
    {
        if (_placingBuilding)
        {
            Destroy(_newBuilding);
        }
        _newBuilding = Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity);
        _placingBuilding = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            set_building(hq);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            set_building(barrack);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
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
