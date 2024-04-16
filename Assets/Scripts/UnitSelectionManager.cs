using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }
    public BuildingSelectionManager buildingManager;

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackble;
    public GameObject groundMarker;

    public bool attackCursorVisible;

    private Camera cam;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            Instance = this;
            
        }

        buildingManager = gameObject.AddComponent<BuildingSelectionManager>();
    }

    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        // left click
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // hitting clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                // shift select multiple units
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }

            }
            else // if not hitting clickable object
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) == false)
                {
                    DeselectAll();
                }

            }
        }
        // right click
        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // hitting clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }


        // attack target
        if (unitsSelected.Count > 0 && AtleastOneOffensiveUnit(unitsSelected))
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // hitting clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackble))
            {
                Debug.Log("Enemy hovered with mouse");

                attackCursorVisible = true;

                if(Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    foreach (GameObject unit in unitsSelected)
                    {
                        if(unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            } else
            {
                attackCursorVisible = false;
            }
        }

    }

    private bool AtleastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }

        }

        return false;
    }

    private void MultiSelect(GameObject unit)
    {
       if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
       else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            SelectUnit(unit, false);
        }

        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    internal void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        
        // if selected is building let building manager handle it
        Unit unitCheck = unit.GetComponent<Unit>();
        if (unitCheck == null) return;
        // default action
        unitsSelected.Add(unit);
        SelectUnit(unit, true);   
        
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }
}
