using TMPro;
using UnityEngine;

namespace UI.BuildingPlacement
{
    public class Button : MonoBehaviour
    {
        private TMP_Text _text;
        private PlaceBuilding _placementManager;
        private ResourceManager _resourceManager;
        [SerializeField] private GameObject building;
        [SerializeField] private int price;

        [SerializeField]
        private new string name;
        
        private void Awake()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _placementManager = PlaceBuilding.Instance;
            _resourceManager = ResourceManager.Instance;
            _text.text = "Name: " + name + "\nPrice: " +
                         price + "\nBuild time: " +
                         "10s";
        }

        public void SetBuilding()
        {
            Debug.Log("order");
            if (_resourceManager.CanAfford(price))
            {
                _placementManager.set_building(building, price);
            }
        }
    }
}
