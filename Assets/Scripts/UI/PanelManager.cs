using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaceBuilding.Instance.mouseOverUI = true;
        mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlaceBuilding.Instance.mouseOverUI = false;
        mouseOverUI = false;
    }
    // I dont like this way of doing this but accessing inactive objectives is impossible otherwise
    
    public static PanelManager Instance;

    public GameObject buildingPanel;
    public GameObject unitProductionPanel;
    public GameObject buildingQue;

    public bool mouseOverUI;
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

        mouseOverUI = false;
    }
}
