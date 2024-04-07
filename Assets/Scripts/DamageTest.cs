using System;
using UnityEngine;

public class DamageTest : MonoBehaviour
{

    private Camera _mainCamera;

    public GameObject bug;
    // Update is called once per frame

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity,
                    LayerMask.GetMask("Attackble"), QueryTriggerInteraction.Collide))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(2);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity,
                    LayerMask.GetMask("Ground")))
            {
                Instantiate(bug, hit.point, Quaternion.identity);
            }
        }
    }
}
