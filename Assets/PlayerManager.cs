using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private bool canInteractSlot = true;
    [SerializeField] private bool isRecoveringInteractSlot = false;

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
        GetListFungusInfo(fungusInfoList);
        Init();
    }
    public void Init()
    {
        fungusSlotListHUD.SetInit(fungusInfoList, inputSlotFungus);
        SwitchFungus(0);
        SetStaminaInit();
    }
    public void GetListFungusInfo(List<FungusInfoReader> fungusInfoList)
    {
        this.fungusInfoList = fungusInfoList;
    }
    public void InputSwitchSlot()
    {
        for (int i = 0; i < inputSlotFungus.Count; i++)
        {
            if (Input.GetKeyDown(inputSlotFungus[i]) &&CanInputSwitchSlot())
            {
                SwitchFungus(i);
            }
        }
    }
    public bool CanInputSwitchSlot()
    {
        return canInteractSlot && !isRecoveringInteractSlot;
    }
    bool CanSwitchFungus(int index)
    {
        if (currentSlotIndex == -1) return true;

        FungusController currentFungus = fungusInfoList[currentSlotIndex].FungusController;
        FungusData newFungusData = fungusInfoList[index].FungusData;

        return currentSlotIndex != index && newFungusData.health != 0 && !currentFungus.IsDying() 
            && !currentFungusInfo.FungusController.IsAttacking();
    }
    public void SwitchFungus(int slotIndex)
    {
        if (!CanSwitchFungus(slotIndex)) return;

        for (int i = 0; i < fungusInfoList.Count; i++)
        {
            if (fungusInfoList[i] == fungusInfoList[slotIndex])
            {
                fungusInfoList[i].gameObject.SetActive(true);
                ReloadCurrentFungus(slotIndex);

                if (recoverySwitchSlotCoroutine != null) StopCoroutine(recoverySwitchSlotCoroutine);
                recoverySwitchSlotCoroutine = StartCoroutine(RecoverySwitchSlotCoroutine());
            }
            else
            {
                fungusInfoList[i].gameObject.SetActive(false);
            }
        }

    }

    //Đặt lại thông tin Fungus sau khi chuyển đổi
    void ReloadCurrentFungus(int index)
    {

        if (currentSlotIndex != -1)
        {
            //Set lại vị trí cho Fungus
            Vector2 currentFungusPos = fungusInfoList[currentSlotIndex].transform.position;
            fungusInfoList[index].transform.position = currentFungusPos;

            //Đặt lại hướng cho Fungus
            FungusController oldFungusController = currentFungusInfo.FungusController;
            FungusController newFungusController = fungusInfoList[index].FungusController;
            newFungusController.SetAttackDirection(oldFungusController.AttackDirection);
            newFungusController.SetLastDirection(oldFungusController.LastDirection);
            newFungusController.SetMoveDirection(oldFungusController.MoveDirection);

        }

        //Đặt lại thông tin và chỉ số
        FungusInfoReader fungusInfo = fungusInfoList[index];
        currentFungusInfo = fungusInfo;
        currentSlotIndex = index;

        //Đặt lại trạng thái đang chọn cho Fungus slot hiện tại
        fungusSlotListHUD.SetSlotSelect(index);

        //thanh thể lực đổi mục tiêu 
        fungusStaminaBar.target = fungusInfoList[index].transform;

        //Đặt lại mục tiêu cho camera
        EventManager.ActionOnCameraChangeTarget(fungusInfoList[index].transform);

        if (currentSlotIndex != -1)
        {
            //Gọi sự  kiện khi được chuyển đổi
            EventManager.ActionOnSwitchFungus(fungusInfoList[currentSlotIndex], fungusInfoList[index], fungusCurrentStatusHUD);
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
            if (slot.CanActive()) slot.SetFade(value);
            else slot.SetFade(GameConfig.inactiveSlotAlpha);

        }
    }
    public IEnumerator RecoverySwitchSlotCoroutine()
    {
        InteractSlotState(false);
        RecoveringInteractSlotState(true);

        SetFadeSlot(GameConfig.fadeSlotAlpha);

        float timer = GameConfig.switchSlotRecoveryTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            SetRecoveryInteractSlot(timer, true);
            yield return null;
        }

        timer = 0;

        InteractSlotState(true);
        RecoveringInteractSlotState(false);

        SetRecoveryInteractSlot(timer, false);

        SetFadeSlot(GameConfig.showSlotAlpha);

    }
    public void RecoveringInteractSlotState(bool state) => isRecoveringInteractSlot = state;
    public void SetRecoveryInteractSlot(float value, bool state)
    {
        foreach (var slot in fungusSlotListHUD.FungusSlotHUDList)
        {
            if (slot.CanActive())
            {
                slot.SetSlotInteractRecoveryState(state);
                slot.SetSlotInteractRecoverySlider(value);
                slot.SetSlotInteractRecoveryTime(value);
            }
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
    public bool CanInteractSlot()
    {
        return canInteractSlot;
    }
    public bool IsRecoveringInteractSlot()
    {
        return isRecoveringInteractSlot;
    }

    void SetStaminaInit()
    {
        currentStamina = PlayerConfig.maxStamina;
        fungusStaminaBar.SetStaminaSliderInit(0, PlayerConfig.maxStamina);
    }
    public void ConsumeStamina(float value) => currentStamina -= value;

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
