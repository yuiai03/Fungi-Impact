using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public Transform target;
    [SerializeField] private Vector3 offset;
    private void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
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
    public void SetStaminaSlider(float currentValue)
    {
        staminaSlider.value = currentValue;
    }

}
