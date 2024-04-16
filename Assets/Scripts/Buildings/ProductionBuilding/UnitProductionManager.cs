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

    public ProductionBuilding ActiveBuilding { get; set; }
    public bool CanBuild { get; set; }

    [SerializeField] private List<BuildableUnit> units = new List<BuildableUnit>();
    public List<BuildableUnit> Units => units;

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
    }

    private void Start()
    {
        _resourceManager = ResourceManager.Instance;
    }

    public void MakeUnit(BuildableUnit unit)
    {
        if (_resourceManager.CanAfford(unit.cost))
        {
            _resourceManager.SpendResource(Resource.Titanium ,unit.cost);
            ActiveBuilding.AddUnitToQue(unit);
        }
        else
        {
            Debug.Log("cannot afford");
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
