using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private bool canInteractSlot = true;

    [Header("HUD")]
    [SerializeField] private FungusCurrentStatusHUD fungusCurrentStatusHUD;
    [SerializeField] private FungusSlotListHUD fungusSlotListHUD;
    [SerializeField] private StaminaBar fungusStaminaBar;

    [Header("Current Fungus Info")]
    [SerializeField] private int currentSlotIndex = -1;
    [SerializeField] private float currentStamina;
    public FungusInfoReader CurrentFungusInfo { get => currentFungusInfo; }
    [SerializeField] private FungusInfoReader currentFungusInfo;

    [Header("Fungus Slot")]
    [SerializeField] private List<FungusInfoReader> fungusInfoList = new List<FungusInfoReader>();
    [SerializeField] private List<KeyCode> inputSlotFungus;

    private Coroutine recoverySwitchSlotCoroutine;
    private Coroutine staminaRecoveryCoroutine;
    private Coroutine updateStaminaBarCoroutine;
    protected override void Awake()
    {
        base.Awake();
        onSpawnFungusInit += OnSpawnFungusInit;
    }
    private void OnDestroy()
    {
        onSpawnFungusInit -= OnSpawnFungusInit;
    }
    public void Update()
    {
        InputSwitchSlot();
    }
    public void OnSpawnFungusInit(List<FungusInfoReader> fungusInfoList)
    {
        SetListFungusInfo(fungusInfoList);
        Init();
    }
    public void Init()
    {
        fungusSlotListHUD.SetInit(fungusInfoList, inputSlotFungus);
        SwitchFungus(0);
        SetStaminaInit();
    }
    public void SetListFungusInfo(List<FungusInfoReader> fungusInfoList)
    {
        this.fungusInfoList = fungusInfoList;
    }
    public void InputSwitchSlot()
    {
        for (int i = 0; i < inputSlotFungus.Count; i++)
        {
            if (Input.GetKeyDown(inputSlotFungus[i]) && canInteractSlot)
            {
                SwitchFungus(i);
            }
        }
    }

    public void SwitchFungus(int slotIndex)
    {
        if (currentSlotIndex == slotIndex || fungusInfoList[slotIndex].FungusData.health == 0) return;

        for (int i = 0; i < fungusInfoList.Count; i++)
        {
            if (fungusInfoList[i] == fungusInfoList[slotIndex])
            {
                if (currentSlotIndex != -1) fungusInfoList[i].transform.position = fungusInfoList[currentSlotIndex].transform.position;

                fungusInfoList[i].gameObject.SetActive(true);

                FungusInfoReader fungusInfo = fungusInfoList[slotIndex];

                currentFungusInfo = fungusInfo;
                currentSlotIndex = slotIndex;

                fungusSlotListHUD.SetSlotSelect(slotIndex);

                EventManager.ActionOnCameraChangeTarget(fungusInfoList[i].transform);

                fungusStaminaBar.target = fungusInfoList[i].transform;

                if (currentSlotIndex == -1) return;
                EventManager.ActionOnSwitchFungus(fungusInfoList[currentSlotIndex], fungusInfoList[i], fungusCurrentStatusHUD);

                if (recoverySwitchSlotCoroutine != null) StopCoroutine(RecoverySwitchSlotCoroutine());
                recoverySwitchSlotCoroutine = StartCoroutine(RecoverySwitchSlotCoroutine());
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

        if (canInteractSlot) SetFadeSlot(GameConfig.showSlotAlpha);
        else SetFadeSlot(GameConfig.fadeSlotAlpha);
    }
    public void SetFadeSlot(float value)
    {
        foreach (var slot in fungusSlotListHUD.FungusSlotHUDList)
        {
            slot.SetFade(value);
        }
    }
    public IEnumerator RecoverySwitchSlotCoroutine()
    {
        canInteractSlot = false;

        SetFadeSlot(GameConfig.fadeSlotAlpha);

        float timer = GameConfig.switchSlotRecoveryTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            SetRecoveryInteractSlot(timer, true);
            yield return null;
        }

        timer = 0;

        canInteractSlot = true;

        SetRecoveryInteractSlot(timer, false);
        SetFadeSlot(GameConfig.showSlotAlpha);

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
            if (fungusData.health != 0)
            {
                return i;
            }
        }
        return -1;
    }



    void SetStaminaInit()
    {
        currentStamina = PlayerConfig.maxStamina;
        fungusStaminaBar.SetStaminaSliderInit(0, PlayerConfig.maxStamina);
    }
    public void ConsumeStamina(float value)
    {
        currentStamina -= value;

    }


    public void UpdateStamina()
    {
        if (currentStamina < PlayerConfig.maxStamina)
        {
            FungusController fungusController = fungusInfoList[currentSlotIndex].GetComponent<FungusController>();

            if (!fungusController.isDashing)
            {
                if (staminaRecoveryCoroutine != null) StopCoroutine(staminaRecoveryCoroutine);
                staminaRecoveryCoroutine = StartCoroutine(StaminaRecoveryCoroutine());
            }

            if (updateStaminaBarCoroutine != null) StopCoroutine(updateStaminaBarCoroutine);
            updateStaminaBarCoroutine = StartCoroutine(UpdateStaminaBarCoroutine());
        }
    }

    IEnumerator UpdateStaminaBarCoroutine()
    {
        //cập nhật thanh thể lực khi dash
        while (Mathf.RoundToInt(fungusStaminaBar.staminaSlider.value) > Mathf.RoundToInt(currentStamina))
        {
            fungusStaminaBar.staminaSlider.value -= PlayerConfig.staminaBarConsumeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator StaminaRecoveryCoroutine()
    {
        fungusStaminaBar.StaminaBarState(true);

        float timer = PlayerConfig.staminaRecoveryWaitTime;

        //thời gian chờ đợi để chờ đợi 
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        //bắt đầu quá trình hồi phục
        while (timer <= 0 && currentStamina < PlayerConfig.maxStamina)
        {

            currentStamina += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;
            
            fungusStaminaBar.staminaSlider.value += PlayerConfig.staminaRecoverySpeed * Time.deltaTime;

            yield return null;
        }

        currentStamina = PlayerConfig.maxStamina;
        fungusStaminaBar.StaminaBarState(false);

    }
    public float GetCurrentStamina()
    {
        return currentStamina;
    }
}
