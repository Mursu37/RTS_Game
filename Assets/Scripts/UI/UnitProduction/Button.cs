using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.UnitProduction
{
    public class Button : MonoBehaviour
    {
        private TMP_Text _text;
        private UnitProductionManager _productionManager;
        [SerializeField] private int index;
        
        private void Awake()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _productionManager = UnitProductionManager.Instance;
            _text.text = "Name: " + _productionManager.Units[index].name + "\nPrice: " +
                         _productionManager.Units[index].cost + "\nBuild time: " +
                         _productionManager.Units[index].timeToBuild;
        }
    }
}
