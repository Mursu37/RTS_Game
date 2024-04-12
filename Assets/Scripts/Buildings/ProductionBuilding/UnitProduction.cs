using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Buildings.ProductionBuilding
{
    public class NewBehaviourScript : MonoBehaviour, IBuilding
    {
        [SerializeField] private List<BuildableUnit> units = new List<BuildableUnit>();
        private List<BuildableUnit> _buildingQue = new List<BuildableUnit>();
        
        private bool _canBuild;
        private float _currentBuildTimer;

        private void Awake()
        {
            _currentBuildTimer = 0f;
        }

        public void BuildingSelected()
        {
            _canBuild = true;
        }

        public void BuildingUnselected()
        {
            _canBuild = false;
        }

        private void CreateNewUnit()
        {
            Instantiate(_buildingQue[0].unit, transform.position, Quaternion.identity);
            _buildingQue.RemoveAt(0);
            _currentBuildTimer = 0f;
        }
        
        private void Update()
        {
            if (_canBuild)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    _buildingQue.Add(units[0]);
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

        [Serializable]
        public class BuildableUnit
        {
            public GameObject unit;
            public int cost;
            public float timeToBuild;
        }
    }
}
