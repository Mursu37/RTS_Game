using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Buildings.ProductionBuilding
{
    public class UnitProduction : MonoBehaviour, IBuilding
    {
        
        private List<BuildableUnit> _buildingQue = new List<BuildableUnit>();
        
        private float _currentBuildTimer;
        private UnitProductionManager _productionManager;
        private GameObject panel;

        private GameObject _buildingQuePanel;
        private TMP_Text _buildingQueText;

        private void Awake()
        {
            _currentBuildTimer = 0f;
            _productionManager = UnitProductionManager.Instance;
            panel = PanelManager.Instance.unitProductionPanel;

            _buildingQuePanel = PanelManager.Instance.buildingQue;
        }

        private void Start()
        {
            _productionManager = UnitProductionManager.Instance;
        }

        public void BuildingSelected()
        {
            panel.SetActive(true);
            
            _buildingQuePanel.SetActive(true);
            _buildingQueText.GetComponentInChildren<TMP_Text>();
            
            _productionManager.CanBuild = true;
           // _productionManager.ActiveBuilding = this;
        }

        public void BuildingUnselected()
        {
            panel.SetActive(false);
            _buildingQuePanel.SetActive(false);
            _productionManager.CanBuild = false;
            _productionManager.ActiveBuilding = null;
        }

        private void CreateNewUnit()
        {
            Instantiate(_buildingQue[0].unit, transform.position + Vector3.down, Quaternion.identity);
            _buildingQue.RemoveAt(0);
            _currentBuildTimer = 0f;
        }


        public void AddUnitToQue(BuildableUnit unit)
        {
            _buildingQue.Add(unit);
        }
        
        private void SetBuildingQueText()
        {
            _buildingQueText.text = "Building Que";

            for (int i = 0; i < _buildingQue.Count && i < 3; i++)
            {
                _buildingQueText.text += "\n" + _buildingQue[i].name;
            }
        }

        private void FixedUpdate()
        {
            if (_buildingQue.Count > 0)
            {
                _currentBuildTimer += Time.fixedDeltaTime;
                if (_currentBuildTimer >= _buildingQue[0].timeToBuild)
                {
                    CreateNewUnit();
                }
            }
        }
    }
}
