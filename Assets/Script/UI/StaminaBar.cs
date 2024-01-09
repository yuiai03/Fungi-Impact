using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    private void Awake()
    {
        staminaSlider = GetComponent<Slider>();
    }
    public void SetStaminaSliderInit(float minValue, float maxValue)
    {
        staminaSlider.minValue = minValue;
        staminaSlider.maxValue = maxValue;
        staminaSlider.value = staminaSlider.maxValue;
    }
    public void UpdateStaminaSlider(float currentValue)
    {
        staminaSlider.value = currentValue;
    }

}
