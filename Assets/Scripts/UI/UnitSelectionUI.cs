using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSelectionUI : MonoBehaviour
{
    public GameObject singleUnitPanel; // Assign in the inspector
    public GameObject multiUnitPanel; // Assign in the inspector
    
 

    private Text singleUnitText; // Assuming there's a Text component on the single unit panel
    private TMP_Text multiUnitText; // Assuming there's a Text component on the multi unit panel

    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text unitHPText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private TMP_Text movementSpeedText;



    void Start()
    {

        
        multiUnitText = multiUnitPanel.GetComponentInChildren<TMP_Text>();
        // Initially hide both panels
        singleUnitPanel.SetActive(false);
        multiUnitPanel.SetActive(false);
    }

    void Update()
    {
        UpdateUI();
    }
    

    void UpdateUI()
    {
        var selectedUnits = UnitSelectionManager.Instance.unitsSelected;
        if (selectedUnits.Count == 1)
        {
            // Only one unit selected, show the single unit view
            singleUnitPanel.SetActive(true);
            multiUnitPanel.SetActive(false);

            Unit selectedUnit = selectedUnits[0].GetComponent<Unit>();
            if (selectedUnit != null)
            {
                UpdateUIForSelectedUnit(selectedUnit);
            }
        }
        else if (selectedUnits.Count > 1)
        {
            // Multiple units selected, show the multiple unit view
            multiUnitText.text = selectedUnits.Count + " Units selected";
            singleUnitPanel.SetActive(false);
            multiUnitPanel.SetActive(true);

        
        }
        else
        {
            // No units selected, hide both views
            singleUnitPanel.SetActive(false);
            multiUnitPanel.SetActive(false);
        }
    }



    public void UpdateUIForSelectedUnit(Unit unit)
    {
        // Update each TMP_Text component with the corresponding unit stat
        unitNameText.text = $"{unit.gameObject.name}"; // currently takes the gameobject name in hierarchy so units are worker, worker (1), worker(2) etc. 
                                                        // TO DO: name variable for each unit
        unitHPText.text = $"{unit.CurrentHealth} / {unit.MaxHealth}";
        damageText.text = $"Damage: {unit.damage}";
        rangeText.text = $"Range: {unit.attackRange}";
        attackSpeedText.text = $"Attack Speed: {unit.attackSpeed}";
        movementSpeedText.text = $"Movement Speed: {unit.movementSpeed}";
    }
}

