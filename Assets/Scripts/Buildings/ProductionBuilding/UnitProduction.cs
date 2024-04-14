using UnityEngine;
using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            _currentBuildTimer = 0f;
            _productionManager = UnitProductionManager.Instance;
            panel = PanelManager.Instance.unitProductionPanel;

        }

        private void Start()
        {
            _productionManager = UnitProductionManager.Instance;
        }

        public void BuildingSelected()
        {
            panel.SetActive(true);
            _productionManager.CanBuild = true;
            _productionManager.ActiveBuilding = this;
        }

        public void BuildingUnselected()
        {
            panel.SetActive(false);
            _productionManager.CanBuild = false;
            _productionManager.ActiveBuilding = null;
        }

        private void CreateNewUnit()
        {
            Instantiate(_buildingQue[0].unit, transform.position, Quaternion.identity);
            _buildingQue.RemoveAt(0);
            _currentBuildTimer = 0f;
        }


        public void AddUnitToQue(BuildableUnit unit)
        {
            _buildingQue.Add(unit);
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
