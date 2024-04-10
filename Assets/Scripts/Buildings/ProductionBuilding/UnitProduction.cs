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
        private bool _canBuild;

        public void BuildingSelected()
        {
            _canBuild = true;
        }

        public void BuildingUnselected()
        {
            _canBuild = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Instantiate(units[0].unit, transform.position, Quaternion.identity);
            }   
        }

        private void FixedUpdate()
        {
        }

        [Serializable]
        public class BuildableUnit
        {
            public GameObject unit;
            public int cost;
        }
    }
}
