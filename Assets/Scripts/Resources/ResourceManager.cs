using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum Resource
{
    Titanium
};

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; set; }
    [SerializeField] private TMP_Text _resourceCounter;
    private Dictionary<Resource, int> _resourceStorage = new Dictionary<Resource, int>();

    private void Awake()
    {
        _resourceStorage.Add(Resource.Titanium, 25);

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
        _resourceCounter.text = "Resource: " + _resourceStorage[Resource.Titanium];
    }

    public void AddResource(Resource resource, int amount)
    {
        _resourceStorage[resource] += amount;
        _resourceCounter.text = "Resource: " + _resourceStorage[Resource.Titanium];
    }

    public void SpendResource(Resource resource ,int amount)
    {
        _resourceStorage[resource] -= amount;
        _resourceCounter.text = "Resource: " + _resourceStorage[Resource.Titanium];
    }
    
    public bool CanAfford(int amount)
    {
        return _resourceStorage[Resource.Titanium] >= amount;
    }
}
