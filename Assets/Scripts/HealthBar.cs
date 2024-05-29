using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider component

    // Method to set the maximum health value of the health bar
    public void setMaxHealth(float health)
    {
        slider.maxValue = health; // Set the maximum value of the slider
        slider.value = health; // Set the current value of the slider to the maximum
    }

    // Method to update the current health value of the health bar
    public void setHealth(float health)
    {
        slider.value = health; // Update the current value of the slider
    }
}

