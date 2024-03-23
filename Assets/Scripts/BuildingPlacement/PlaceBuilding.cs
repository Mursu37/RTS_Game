using System;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject turret;
    public GameObject hq;
    public GameObject barrack;
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void place_building(GameObject building)
    {
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            GameObject new_building = Instantiate(building, hit.point, Quaternion.identity);
                
            // Look for objects at position not on Ground layer
            Collider[] collisions = Physics.OverlapBox(new_building.transform.position,
                new_building.GetComponentInChildren<BoxCollider>().bounds.extents, Quaternion.identity,
                ~LayerMask.GetMask("Ground"));
                
            // Collision box always collides with newly created object so we check if more than one collision occured
            if (collisions.Length > 1)
            {
                // Destroy object if collision with other objects with colliders happened
                Destroy(new_building);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            place_building(hq);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            place_building(barrack);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            place_building(turret);
        }
        
    }
    
}
