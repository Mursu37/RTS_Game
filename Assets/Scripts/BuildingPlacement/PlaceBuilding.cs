using System;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject turret;
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GameObject new_turret = Instantiate(turret, hit.point, Quaternion.identity);
                
                // Look for objects at position not on Ground layer
                Collider[] collisions = Physics.OverlapBox(new_turret.transform.position,
                    new_turret.GetComponentInChildren<BoxCollider>().bounds.extents, Quaternion.identity,
                    ~LayerMask.GetMask("Ground"));
                
                // Collision box always collides with newly created object so we check if more than one collision occured
                if (collisions.Length > 1)
                {
                    // Destroy object if collision with other objects with colliders happened
                    Destroy(new_turret);
                }
            }
        }
    }
    
}
