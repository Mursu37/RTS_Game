using Buildings;
using UnityEngine;

public class BuildingSelectionManager : MonoBehaviour
{
    private bool _buildingIsSelected;
    private GameObject _selectedBuilding;

    private void Awake()
    {
        _buildingIsSelected = false;
        _selectedBuilding = null;
    }

    public void SelectBuilding(GameObject building)
    {
        if (_buildingIsSelected)
        {
            UnSelectBuilding();
        }
        
        IBuilding buildingInterface = building.GetComponent<IBuilding>();
        buildingInterface.BuildingSelected();
        _selectedBuilding = building;
        _buildingIsSelected = true;
        Debug.Log("Building is selected");
    }

    public void UnSelectBuilding()
    {
        IBuilding buildingInterface = _selectedBuilding.GetComponent<IBuilding>();
        buildingInterface.BuildingUnselected();
        _selectedBuilding = null;
        _buildingIsSelected = false;
        Debug.Log("Building is unselected");
    }

    public void Update()
    {
        if (_buildingIsSelected)
        {
            // unselect building only if a new building or escape is pressed.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnSelectBuilding();
            }
        }
    }
}
