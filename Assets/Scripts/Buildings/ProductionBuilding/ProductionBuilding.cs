using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.AI;

namespace Buildings.ProductionBuilding
{
    public class ProductionBuilding : Building
    {
        private List<BuildableUnit> _buildingQue = new List<BuildableUnit>();
        
        private float _currentBuildTimer;
        private UnitProductionManager _productionManager;
        private GameObject panel;

        private GameObject _buildingQuePanel;
        private TMP_Text _buildingQueText;

        private bool _active;
        private Camera _mainCamera;
        private Vector3 _rallyPoint;
        [SerializeField] private GameObject rallyPointVisual;
        
        private void Awake()
        {
            MaxHealth = 350f;
            _currentBuildTimer = 0f;
            _active = false;
            _mainCamera = Camera.main;
            
            rallyPointVisual.SetActive(false);
        }
        
        protected override void Start()
        {
            base.Start();
            _productionManager = UnitProductionManager.Instance;
            panel = PanelManager.Instance.unitProductionPanel;
            _buildingQuePanel = PanelManager.Instance.buildingQue;
        }

        public override void BuildingSelected()
        {
            // Move panels to building selection manager?
            panel.SetActive(true);
            
            _buildingQuePanel.SetActive(true);
            _buildingQueText = _buildingQuePanel.GetComponentInChildren<TMP_Text>();
            SetBuildingQueText();
            
            _productionManager.CanBuild = true;
            _productionManager.ActiveBuilding = this;

            _active = true;
            rallyPointVisual.SetActive(true);
        }

        public override void BuildingUnselected()
        {
            panel.SetActive(false);
            _buildingQuePanel.SetActive(false);
            _productionManager.CanBuild = false;
            _productionManager.ActiveBuilding = null;

            _active = false;
            rallyPointVisual.SetActive(false);
        }

        private void CreateNewUnit()
        {
            // get size of building and unit to calculate spawning position
            Vector3 size = _buildingQue[0].unit.GetComponentInChildren<Collider>().bounds.extents +
                           GetComponent<Collider>().bounds.extents;
            
            var newUnit = Instantiate(_buildingQue[0].unit, transform.position - new Vector3(0, 0, size.z + 0.5f), Quaternion.identity);
            if (_rallyPoint != Vector3.zero) newUnit.GetComponentInChildren<NavMeshAgent>().SetDestination(_rallyPoint);
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

        private void Update()
        {
            if (_active)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,
                            Mathf.Infinity,
                            LayerMask.GetMask("Ground")))
                    {
                        _rallyPoint = hit.point;
                        rallyPointVisual.transform.position = hit.point;
                    }
                }

                if (_buildingQue.Count > 0)
                {
                    UI.UnitProduction.UnitProductionProgressBar.Instance.UpdateBarProgress(_currentBuildTimer, _buildingQue[0].timeToBuild);
                }
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
