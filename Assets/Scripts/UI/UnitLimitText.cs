using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitLimitText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text unitLimitText;
    private UnitSelectionManager _selectionManager;
    private UnitProductionManager _productionManager;
    private void Start()
    {
        _selectionManager = UnitSelectionManager.Instance;
        _productionManager = UnitProductionManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        unitLimitText.text = "Units " + _selectionManager.allUnitsList.Count +  " / " +  _productionManager.UnitLimit;
    }
}
