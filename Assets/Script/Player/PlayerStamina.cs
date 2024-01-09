using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{

    public float currentStamina;
    public float maxStamina = 100;

    [SerializeField] private float countRecoveryTime = 0;
    [SerializeField] private StaminaBar staminaBar;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        SetStaminaInit();
    }
    void SetStaminaInit()
    {
        currentStamina = maxStamina;
        staminaBar.SetStaminaSliderInit(0, maxStamina);
    }
    public void UpdateStamina(float value)
    {
        currentStamina -= value;
    }
    void StaminaRecovery()
    {
        countRecoveryTime += Time.deltaTime;
        if (countRecoveryTime >= PlayerConfig.staminaRecoveryWaitTime && currentStamina < maxStamina)
        {
            currentStamina += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public void UpdateStaminaBarState()
    {
        if(currentStamina >= maxStamina)
        {
            if (staminaBar.gameObject.activeSelf)
            {
                staminaBar.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!staminaBar.gameObject.activeSelf)
            {
                staminaBar.gameObject.SetActive(true);
            }
        }
    }
    public void UpdateStaminaBar()
    {
        if (Mathf.RoundToInt(staminaBar.staminaSlider.value) >= Mathf.RoundToInt(currentStamina))
        {
            staminaBar.staminaSlider.value -=  PlayerConfig.staminaBarRecoverySpeed * Time.deltaTime;
        }
        else
        {
            staminaBar.staminaSlider.value += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public void UpdateStaminaState()
    {
        if (currentStamina < maxStamina)
        {
            if (!playerController.isDashing)
            {
                StaminaRecovery();
            }
            else
            {
                countRecoveryTime = 0;
            }
        }
    }
}
