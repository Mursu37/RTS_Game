using System.Collections.Generic;
using UnityEngine;
public enum Resource
{
    Titanium
};

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; set; }
    private Dictionary<Resource, int> _resourceStorage = new Dictionary<Resource, int>();

    private void Awake()
    {
        _resourceStorage.Add(Resource.Titanium, 500);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddResource(Resource resource, int amount)
    {
        _resourceStorage[resource] += amount;
    }

    public void SpendResource(Resource resource ,int amount)
    {
        _resourceStorage[resource] -= amount;
    }
    
    public bool CanAfford(int amount)
    {
        return _resourceStorage[Resource.Titanium] >= amount;
    }
}
