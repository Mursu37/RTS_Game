using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings.SupplyDepo
{
    public class NewBehaviourScript : Building
    {
        private int _unitIncreaseAmount;
        private UnitProductionManager _productionManager;
        
        private void Awake()
        {
            MaxHealth = 250f;
        }

        protected override void Start()
        {
            base.Start();
            _unitIncreaseAmount = 5;
            _productionManager = UnitProductionManager.Instance;
            _productionManager.UnitLimit += _unitIncreaseAmount;
        }

        private void OnDestroy()
        {
            if (this.enabled) _productionManager.UnitLimit -= _unitIncreaseAmount;
        }
    }
}
