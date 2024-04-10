
using System;
using UnityEngine;
public class TitaniumNode : MonoBehaviour, IGatherable
{
    private int _resourcesBeforeDepletion = 75;
    public Resource ResourceType { get; set; } = Resource.Titanium;
    public int Gather()
    {
        _resourcesBeforeDepletion -= 1;
        if (_resourcesBeforeDepletion <= 0)
        {
            Destroy(gameObject);
        }
        return 1;
    }
}
