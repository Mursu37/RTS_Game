using System;
using System.Collections;
using System.Collections.Generic;
using Buildings.ProductionBuilding;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitProductionManager : MonoBehaviour
{
    public static UnitProductionManager Instance { get; set; }
    
    private ResourceManager _resourceManager;
    private UnitSelectionManager _selectionManager;

    public ProductionBuilding ActiveBuilding { get; set; }
    public bool CanBuild { get; set; }

    [SerializeField] private List<BuildableUnit> units = new List<BuildableUnit>();
    public List<BuildableUnit> Units => units;
    
    // unit limit
    public int UnitLimit { get; set; }
    public int UnitsInQue { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        UnitLimit = 10;
        UnitsInQue = 0;
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
        _selectionManager = UnitSelectionManager.Instance;
    }

    public void MakeUnit(BuildableUnit unit)
    {
        if (_resourceManager.CanAfford(unit.cost) && _selectionManager.allUnitsList.Count + UnitsInQue < UnitLimit)
        {
            _resourceManager.SpendResource(Resource.Titanium ,unit.cost);
            ActiveBuilding.AddUnitToQue(unit);
            UnitsInQue++;
        }
        else
        {
            Debug.Log("cannot afford or unit limit reached");
        }
    }
}
[Serializable]
public class BuildableUnit
{
    public string name;
    public GameObject unit;
    public int cost;
    public float timeToBuild;
}
