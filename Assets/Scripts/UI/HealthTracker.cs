using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public Slider HealthBarSlider;
    public Image sliderFill;

    public Material greenEmission;
    public Material yellowEmission;
    public Material redEmission;

    // Call this method to update the health bar and color
    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        // Calculate the health percentage
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Update the slider value
        HealthBarSlider.value = healthPercentage;

        // Update the color based on health percentage
        UpdateColor(healthPercentage);
    }

    // Set the color based on the health percentage
    private void UpdateColor(float healthPercentage)
    {
        if (healthPercentage >= 0.6f)
        {
            sliderFill.material = greenEmission;
        }
        else if (healthPercentage >= 0.3f)
        {
            sliderFill.material = yellowEmission;
        }
        else
        {
            sliderFill.material = redEmission;
        }
    }
}

