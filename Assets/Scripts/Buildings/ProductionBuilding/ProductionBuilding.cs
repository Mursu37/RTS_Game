using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Buildings.ProductionBuilding
{
    public class ProductionBuilding : MonoBehaviour, IDamageable, IBuilding
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        private void Awake()
        {
            MaxHealth = 350f;
            CurrentHealth = MaxHealth;
            
            _currentBuildTimer = 0f;
        }

        public void Damage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth < 0)
            {
                Die();
            }
        }

        public void Die()
        {
            BuildingUnselected();
            Destroy(gameObject);
        }
        
         private List<BuildableUnit> _buildingQue = new List<BuildableUnit>();
        
        private float _currentBuildTimer;
        private UnitProductionManager _productionManager;
        private GameObject panel;

        private GameObject _buildingQuePanel;
        private TMP_Text _buildingQueText;

        private void Start()
        {
            _productionManager = UnitProductionManager.Instance;
            panel = PanelManager.Instance.unitProductionPanel;
            _buildingQuePanel = PanelManager.Instance.buildingQue;
        }

        public void BuildingSelected()
        {
            panel.SetActive(true);
            
            _buildingQuePanel.SetActive(true);
            _buildingQueText = _buildingQuePanel.GetComponentInChildren<TMP_Text>();
            SetBuildingQueText();
            
            _productionManager.CanBuild = true;
            _productionManager.ActiveBuilding = this;
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
            SetBuildingQueText();
        }


        public void AddUnitToQue(BuildableUnit unit)
        {
            _buildingQue.Add(unit);
            SetBuildingQueText();
        }
        
        private void SetBuildingQueText()
        {
            _buildingQueText.text = "Building Que";

            for (int i = 0; i < _buildingQue.Count && i < 3; i++)
            {
                int order = i + 1;
                _buildingQueText.text += "\n"+ order + ": " + _buildingQue[i].name;
            }

            if (_buildingQue.Count > 3)
            {
                int amountMore = _buildingQue.Count - 3;
                _buildingQueText.text += "\n" + amountMore + " more units";
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
