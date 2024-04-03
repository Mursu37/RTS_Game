
using UnityEngine;

public enum Resource
{
    Titanium
};
public class TitaniumNode : MonoBehaviour, IGatherable
{
    public Resource ResourceType { get; set; } = Resource.Titanium;
    
    
    public int Gather()
    {
        return 1;
    }
}
