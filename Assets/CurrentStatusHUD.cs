using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentStatusHUD : MonoBehaviour
{
    public Slider currentHealthSlider;
    public Slider currentDamageSlider;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI lvText;

    public void SetCurrentHealthSliderInit(float minValue, float maxValue)
    {
        currentHealthSlider.minValue = minValue;
        currentHealthSlider.maxValue = maxValue;
    }
    public void SetCurrentDamageSliderInit(float minValue, float maxValue)
    {
        currentDamageSlider.minValue = minValue;
        currentDamageSlider.maxValue = maxValue;
    }
    public void SetCurrentHealthSlider(float value) => currentHealthSlider.value = value;
    public void SetCurrentDamageSlider(float value) => currentDamageSlider.value = value;
    public void SetHealthText(float value, float maxValue) => healthText.text = value + " / " + maxValue;
    public void SetLvText(int lvValue) => lvText.text = "Lv." + lvValue.ToString();
}
