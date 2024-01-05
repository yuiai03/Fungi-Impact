using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{

    public float countRecoveryTime = 0;
    public float recoverySpeed = 20;
    public float consumeSpeed = 50;
    public float recoveryTime = 2;

    public float currentStamina;
    public float maxStamina = 100;
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
    private void OnDestroy()
    {
    }
    private void Update()
    {
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
    public void StaminaRecovery()
    {
        countRecoveryTime += Time.deltaTime;
        if (countRecoveryTime >= recoveryTime && currentStamina < maxStamina)
        {
            currentStamina += recoverySpeed * Time.deltaTime;
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
            staminaBar.staminaSlider.value -= consumeSpeed * Time.deltaTime;
        }
        else
        {
            staminaBar.staminaSlider.value += recoverySpeed * Time.deltaTime;
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
