
using System;
using TMPro;
using UnityEngine;
public class TitaniumNode : MonoBehaviour, IGatherable
{
    private int _resourcesBeforeDepletion = 500;
    [SerializeField] private TMP_Text resourceCount;
    public Resource ResourceType { get; set; } = Resource.Titanium;

    private void Start()
    {
        resourceCount.text = "" + _resourcesBeforeDepletion;
    }

    public int Gather()
    {
        _resourcesBeforeDepletion -= 1;
        resourceCount.text = "" + _resourcesBeforeDepletion;
        if (_resourcesBeforeDepletion <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
        return 1;
    }

    public void SetNearNode()
    {
        _resourcesBeforeDepletion = 275;
    }
}
