using System.Collections.Generic;
using UnityEngine;
public enum Resource
{
    Titanium
};

public class ResourceManager : MonoBehaviour
{
    private Dictionary<Resource, int> _resourceStorage = new Dictionary<Resource, int>();

    private void Awake()
    {
        _resourceStorage.Add(Resource.Titanium, 0);
    }

    public void AddResource(Resource resource, int amount)
    {
        _resourceStorage[resource] += amount;
        Debug.Log("Resource count: " + _resourceStorage[resource]);
    }

    public void SpendResource(Resource resource ,int amount)
    {
        _resourceStorage[resource] += amount;
    }
}
