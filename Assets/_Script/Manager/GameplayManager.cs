using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private bool canInteractSlot = true;
    [SerializeField] private bool needRecoveryInteractSlot = false;
    [SerializeField] private float countSwitchSlotRecoveryTime = 0;
    [SerializeField] private float countRecoveryStaminaTime = 0;

    [Header("HUD")]
    [SerializeField] private FungusCurrentStatusHUD fungusCurrentStatusHUD;
    [SerializeField] private FungusSlotListHUD fungusSlotListHUD;
    [SerializeField] private StaminaBar fungusStaminaBar;

    [Header("Current Fungus Info")]
    [SerializeField] private int currentSlotIndex = -1;
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float currentStamina;
    public FungusInfoReader CurrentFungusInfo { get => currentFungusInfo; }
    [SerializeField] private FungusInfoReader currentFungusInfo;

    [Header("Fungus Slot")]
    [SerializeField] private List<FungusInfoReader> fungusInfoList;
    [SerializeField] private List<KeyCode> inputSlotFungus;

    [SerializeField] private Transform root; //obj player holder
    private ManagerRoot managerRoot => ManagerRoot.Instance;

    public void Start()
    {
        Init();
    }
    public void Update()
    {
        InputSwitchSlot();
        RecoverySwitchSlot();
    }
    public void Init()
    {
        SpawnFungusInit();
        fungusSlotListHUD.SetInit(fungusInfoList, inputSlotFungus);
        SwitchFungus(0);
        SetStaminaInit();
    }
    public void InputSwitchSlot()
    {
        for (int i = 0; i < inputSlotFungus.Count; i++)
        {
            if (Input.GetKeyDown(inputSlotFungus[i]) && canInteractSlot && !needRecoveryInteractSlot)
            {
                SwitchFungus(i);
            }
        }
    }
    public void SpawnFungusInit()
    {
        List<FungusNameType> fungusNameTypeList = managerRoot.actionFungusNameList;
        AvailableFungiConfig availableFungiConfig = managerRoot.ManagerRootConfig.availableFungiConfig;

        foreach (FungusNameType fungusNameType in fungusNameTypeList)
        {
            FungusPackedConfig fungusPackedConfig = availableFungiConfig.GetFungusPackedConfigByNameType(fungusNameType);

            FungusInfoReader fungus = Instantiate(fungusPackedConfig.fungusInfoReader, root);

            FungusData fungusData = new FungusData();

            fungusData.maxHealth = fungusPackedConfig.stats.maxHealth;
            fungusData.health = fungusPackedConfig.stats.maxHealth;

            fungusData.damage = fungusPackedConfig.stats.damage;
            fungusData.lv = fungusPackedConfig.stats.lv;
            fungusData.moveSpeed = fungusPackedConfig.stats.moveSpeed;

            fungusData.dashTime = fungusPackedConfig.stats.dashTime;
            fungusData.dashForce = fungusPackedConfig.stats.dashForce;
            fungusData.dashStamina = fungusPackedConfig.stats.dashStamina;

            fungusData.fungusConfig = fungusPackedConfig.config;
            fungusData.fungusStats = fungusPackedConfig.stats;

            fungus.GetData(fungusData);

            fungusInfoList.Add(fungus);
        }
    }
    public void SwitchFungus(int slotIndex)
    {
        if (currentSlotIndex == slotIndex || fungusInfoList[slotIndex].FungusData.health == 0) return;

        for (int i = 0; i < fungusInfoList.Count; i++)
        {
            if (fungusInfoList[i] == fungusInfoList[slotIndex])
            {
                if(currentSlotIndex != -1) fungusInfoList[i].transform.position = fungusInfoList[currentSlotIndex].transform.position;

                fungusInfoList[i].gameObject.SetActive(true);

                FungusInfoReader fungusInfo = fungusInfoList[slotIndex];

                currentFungusInfo = fungusInfo;
                currentSlotIndex = slotIndex;

                fungusSlotListHUD.SetSlotSelect(slotIndex);

                NeedRecoveryInteractSlotState(true);

                EventManager.ActionOnCameraChangeTarget(fungusInfoList[i].transform);

                fungusStaminaBar.target = fungusInfoList[i].transform;

                if (currentSlotIndex == -1) return;
                EventManager.ActionOnSwitchFungus(fungusInfoList[currentSlotIndex], fungusInfoList[i], fungusCurrentStatusHUD);
            }
            else
            {
                fungusInfoList[i].gameObject.SetActive(false);
            }
        }

    }

    public void InteractSlotState(bool state)
    {
        canInteractSlot = state;

        if (needRecoveryInteractSlot) return;

        if (canInteractSlot) SetFadeSlot(GameConfig.showSlotAlpha);
        else SetFadeSlot(GameConfig.fadeSlotAlpha);
    }
    public void SetFadeSlot(float value)
    {
        foreach(var slot in fungusSlotListHUD.FungusSlotHUDList)
        {
            slot.SetFade(value);
        }
    }
    public void RecoverySwitchSlot()
    {
        if (needRecoveryInteractSlot)
        {
            countSwitchSlotRecoveryTime += Time.deltaTime;
            SetRecoveryInteractSlot(countSwitchSlotRecoveryTime, true);
            if (countSwitchSlotRecoveryTime >= GameConfig.switchSlotRecoveryTime)
            {
                countSwitchSlotRecoveryTime = 0;
                NeedRecoveryInteractSlotState(false);
                SetRecoveryInteractSlot(countSwitchSlotRecoveryTime, false);
            }
        }
    }
    void NeedRecoveryInteractSlotState(bool state)
    {
        needRecoveryInteractSlot = state;

        if (!canInteractSlot) return;

        if (!needRecoveryInteractSlot) SetFadeSlot(GameConfig.showSlotAlpha);
        else SetFadeSlot(GameConfig.fadeSlotAlpha);
    }

    public void SetRecoveryInteractSlot(float value, bool state)
    {
        foreach (var slot in fungusSlotListHUD.FungusSlotHUDList)
        {
            slot.SetSlotInteractRecoveryState(state);
            slot.SetSlotInteractRecoverySlider(value);
            slot.SetSlotInteractRecoveryTime(value);
        }
    }
    public int GetFungusAliveIndex()
    {
        for (int i = 0; i < fungusInfoList.Count; i++)
        {
            FungusData fungusData = fungusInfoList[i].FungusData;
            if(fungusData.health != 0)
            {
                return i;
            }
        }
        return -1;
    }

    void SetStaminaInit()
    {
        currentStamina = maxStamina;
        fungusStaminaBar.SetStaminaSliderInit(0, maxStamina);
    }
    public void ConsumeStamina(float value)
    {
        currentStamina -= value;
    }

    public void UpdateStaminaBarState()
    {
        if (currentStamina >= maxStamina)
        {
            if (fungusStaminaBar.gameObject.activeSelf)
            {
                fungusStaminaBar.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!fungusStaminaBar.gameObject.activeSelf)
            {
                fungusStaminaBar.gameObject.SetActive(true);
            }
        }
    }
    public void UpdateStaminaBar()
    {
        if (Mathf.RoundToInt(fungusStaminaBar.staminaSlider.value) > Mathf.RoundToInt(currentStamina))
        {
            fungusStaminaBar.staminaSlider.value -= PlayerConfig.staminaBarRecoverySpeed * Time.deltaTime;
        }
        else if (Mathf.RoundToInt(fungusStaminaBar.staminaSlider.value) < Mathf.RoundToInt(currentStamina))
        {
            fungusStaminaBar.staminaSlider.value += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public void UpdateStaminaState()
    {
        if (currentStamina < maxStamina)
        {
            FungusController fungusController = fungusInfoList[currentSlotIndex].GetComponent<FungusController>();
            if (fungusController == null) return;

            if (!fungusController.isDashing)
            {
                StaminaRecovery();
            }
            else
            {
                countRecoveryStaminaTime = 0;
            }
        }
    }
    void StaminaRecovery()
    {
        countRecoveryStaminaTime += Time.deltaTime;
        if (countRecoveryStaminaTime >= PlayerConfig.staminaRecoveryWaitTime && currentStamina < maxStamina)
        {
            currentStamina += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
        }
    }
    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}
