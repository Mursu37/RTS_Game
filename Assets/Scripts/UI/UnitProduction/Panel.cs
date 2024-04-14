using UnityEngine;

namespace UI.UnitProduction
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingPanel;
        
        private void OnDisable()
        {
            _buildingPanel.SetActive(true);
        }

        private void OnEnable()
        {
            _buildingPanel.SetActive(false);
        }
    }
}
