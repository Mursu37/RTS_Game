using System.Net;
using Buildings;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    public bool BuildingIsActive { get; private set; }
    public Collider ActiveBuilding { get; private set; }
    
    private Camera _camera;

    private bool _buildingIsClicked;
    private Collider _buildingClicked;

    private void Awake()
    {
        BuildingIsActive = false;
        ActiveBuilding = null;
        _camera = Camera.main;
    }

    public void SelectBuilding(Collider building)
    {
        if (BuildingIsActive)
        {
            UnSelectBuilding();
        }
        
        IBuilding buildingInterface = building.GetComponent<IBuilding>();
        buildingInterface.BuildingSelected();
        ActiveBuilding = building;
        BuildingIsActive = true;
    }

    public void UnSelectBuilding()
    {
        IBuilding buildingInterface = ActiveBuilding.GetComponent<IBuilding>();
        buildingInterface.BuildingUnselected();
        ActiveBuilding = null;
        BuildingIsActive = false;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            // hitting clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable")))
            {
                if (hit.collider.transform.CompareTag("Buildable")) return;
                var building = hit.collider.GetComponent<IBuilding>();
                if (building != null)
                {
                    _buildingClicked = hit.collider;
                    _buildingIsClicked = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && _buildingIsClicked)
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            // hitting clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable")))
            {
                if (hit.collider == _buildingClicked)
                {
                    SelectBuilding(_buildingClicked);
                }
            }

            _buildingIsClicked = false;
            _buildingClicked = null;
        }

        if (BuildingIsActive)
        {
            // unselect building only if a new building or escape is pressed.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnSelectBuilding();
            }
        }
    }
}
