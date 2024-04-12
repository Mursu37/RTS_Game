using Buildings;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    private bool _buildingIsActive;
    private Collider _activeBuilding;
    private Camera _camera;

    private bool _buildingIsClicked;
    private Collider _buildingClicked;

    private void Awake()
    {
        _buildingIsActive = false;
        _activeBuilding = null;
        _camera = Camera.main;
    }

    public void SelectBuilding(Collider building)
    {
        if (_buildingIsActive)
        {
            UnSelectBuilding();
        }
        
        IBuilding buildingInterface = building.GetComponent<IBuilding>();
        buildingInterface.BuildingSelected();
        _activeBuilding = building;
        _buildingIsActive = true;
        Debug.Log("Building is selected");
    }

    public void UnSelectBuilding()
    {
        IBuilding buildingInterface = _activeBuilding.GetComponent<IBuilding>();
        buildingInterface.BuildingUnselected();
        _activeBuilding = null;
        _buildingIsActive = false;
        Debug.Log("Building is unselected");
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
                var building = hit.collider.GetComponent<IBuilding>();
                if (building != null)
                {
                    _buildingClicked = hit.collider;
                    _buildingIsClicked = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && _buildingClicked)
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

        if (_buildingIsActive)
        {
            // unselect building only if a new building or escape is pressed.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnSelectBuilding();
            }
        }
    }
}
