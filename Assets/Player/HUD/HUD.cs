using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class HUD : MonoBehaviour
{
    public GUISkin resourceSkin, ordersSkin;
    private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 120;
    private Player player;
    private Dictionary<ResourceType, int> resourceValues, resourceLimits;
    private const int ICON_WIDTH = 100, ICON_HEIGHT = 100, TEXT_WIDTH = 256, TEXT_HEIGHT = 64;
    public Texture2D[] resources;
    private Dictionary<ResourceType, Texture2D> resourceImages;

    // Start is called before the first frame update

    void Start()
    {
        resourceValues = new Dictionary<ResourceType, int>();
        resourceLimits = new Dictionary<ResourceType, int>();
        player = transform.root.GetComponent<Player>();
        resourceImages = new Dictionary<ResourceType, Texture2D>();
        for (int i = 0; i < resources.Length; i++)
        {
            switch (resources[i].name)
            {
                case "Money":
                    resourceImages.Add(ResourceType.Money, resources[i]);
                    resourceValues.Add(ResourceType.Money, 0);
                    resourceLimits.Add(ResourceType.Money, 0);
                    break;
                case "Power":
                    resourceImages.Add(ResourceType.Power, resources[i]);
                    resourceValues.Add(ResourceType.Power, 0);
                    resourceLimits.Add(ResourceType.Power, 0);
                    break;
                default: break;
            }
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (player && player.human)
        {
            DrawOrdersBar();
            DrawResourceBar();
        }
    }

    private void DrawOrdersBar()
    {
        GUI.skin = ordersSkin;
        GUI.BeginGroup(new Rect(Screen.width - ORDERS_BAR_WIDTH, RESOURCE_BAR_HEIGHT, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT), "");
        GUI.EndGroup();
    }

    private void DrawResourceBar()
    {
        GUI.skin = resourceSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");
        int topPos = 20, iconLeft = 60, textLeft = 20;
        DrawResourceIcon(ResourceType.Money, iconLeft, textLeft, topPos);
        iconLeft += TEXT_WIDTH;
        textLeft += TEXT_WIDTH;
        DrawResourceIcon(ResourceType.Power, iconLeft, textLeft, topPos);

        GUI.EndGroup();
    }

    public void SetResourceValues(Dictionary<ResourceType, int> resourceValues, Dictionary<ResourceType, int> resourceLimits)
    {
        this.resourceValues = resourceValues;
        this.resourceLimits = resourceLimits;
    }

    private void DrawResourceIcon(ResourceType type, int iconLeft, int textLeft, int topPos)
    {
        Texture2D icon = resourceImages[type];
        string text = resourceValues[type].ToString() + "/" + resourceLimits[type].ToString();
        GUI.DrawTexture(new Rect(iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), icon);
        GUI.Label(new Rect(textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
    }
}
