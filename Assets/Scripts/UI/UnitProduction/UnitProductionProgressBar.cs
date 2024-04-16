using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UnitProduction
{
    public class UnitProductionProgressBar : MonoBehaviour
    {
        public static UnitProductionProgressBar Instance { get; set; }
        private Image _bar;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _bar = GetComponent<Image>();
        }

        /// <summary>
        /// current progress divided by total amount needed to finish
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        public void UpdateBarProgress(float current, float total)
        {
            _bar.fillAmount = current / total;
        }
    }
}
