using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private bool canInteractSlot = true;
    [SerializeField] private bool needRecoveryInteractSlot;
    [SerializeField] private float countSwitchSlotRecoveryTime = 0;

    [SerializeField] private PlayerConfigReader playerConfig;
    [SerializeField] private FungusSlotListHUD fungusSlotListHUD;

    [Header("Current Fungus Info")]
    [SerializeField] private int currentSlotIndex = -1;
    [SerializeField] private FungusData currentFungusData;
    [SerializeField] private FungusConfig currentFungusConfig;

    [Header("Fungus Slot")]
    [SerializeField] private List<FungusData> fungusDataList;
    [SerializeField] private List<KeyCode> inputSlotFungus;
    private ManagerRoot managerRoot => ManagerRoot.instance;
    public static GameplayController instance;
    private void Awake()
    {
        instance = this;
    }
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
        GetListDataFungusSlotInit();
        fungusSlotListHUD.SetInit(fungusDataList, inputSlotFungus);

        SwitchFungus(0);
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
    public void GetListDataFungusSlotInit()
    {

        List<FungusNameType> fungusNameTypeList = managerRoot.actionFungusNameList;
        AvailableFungiConfig availableFungiConfig = managerRoot.ManagerRootConfig.availableFungiConfig;

        foreach (FungusNameType fungusNameType in fungusNameTypeList)
        {
            FungusPackedConfig fungusPackedConfig = availableFungiConfig.GetFungusPackedConfigByNameType(fungusNameType);

            FungusData fungusData = new FungusData();
            fungusData.maxHealth = fungusPackedConfig.stats.fungusMaxHealth;
            fungusData.health = fungusPackedConfig.stats.fungusMaxHealth;
            fungusData.damage = fungusPackedConfig.stats.fungusDamage;
            fungusData.fungusConfig = fungusPackedConfig.config;
            fungusData.fungusStats = fungusPackedConfig.stats;

            fungusDataList.Add(fungusData);
        }
    }
    public void SwitchFungus(int slotIndex)
    {
        if (currentSlotIndex == slotIndex) return;

        FungusData fungusData = fungusDataList[slotIndex];
        currentFungusData = fungusData;
        currentSlotIndex = slotIndex;
        currentFungusConfig = fungusData.fungusConfig;

        fungusSlotListHUD.SetSlotSelect(slotIndex);

        ReloadPlayerConfig();

        NeedRecoveryInteractSlotState(true);

        EventManager.ActionOnSwitchFungus(fungusData);
    }
    public void ReloadPlayerConfig()
    {
        playerConfig.GetConfig(currentFungusConfig);
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
}
