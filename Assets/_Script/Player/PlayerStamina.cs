using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina;
    public float stamina;

    [SerializeField] private float countRecoveryTime = 0;
    [SerializeField] private StaminaBar staminaBar;
    private PlayerController playerController;
    private PlayerInfoReader playerInfo;

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfoReader>();
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        SetStaminaInit();
    }

    void SetStaminaInit()
    {
        stamina = maxStamina;
        staminaBar.SetStaminaSliderInit(0, maxStamina);
    }
    public void ConsumeStamina(float value)
    {
        stamina -= value;
    }
    void StaminaRecovery()
    {
        countRecoveryTime += Time.deltaTime;
        if (countRecoveryTime >= PlayerConfig.staminaRecoveryWaitTime && stamina < maxStamina)
        {
            stamina += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public void UpdateStaminaBarState()
    {
        if(stamina >= maxStamina)
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
        if (Mathf.RoundToInt(staminaBar.staminaSlider.value) > Mathf.RoundToInt(stamina))
        {
            staminaBar.staminaSlider.value -=  PlayerConfig.staminaBarRecoverySpeed * Time.deltaTime;
        }
        else if(Mathf.RoundToInt(staminaBar.staminaSlider.value) < Mathf.RoundToInt(stamina))
        {
            staminaBar.staminaSlider.value += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public void UpdateStaminaState()
    {
        if (stamina < maxStamina)
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
