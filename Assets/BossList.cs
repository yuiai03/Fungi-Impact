using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossList : MonoBehaviour
{
    [SerializeField] private BossSlot bossSlotPrefab;
    public List<BossSlot> bossSlotList = new List<BossSlot>();
    private ManagerRoot managerRoot => ManagerRoot.instance;


    private void Start()
    {
        EventManager.onSelectSlotBoss += OnSelectSlotBoss;
        Init();
       
    }
    private void OnDestroy()
    {
        EventManager.onSelectSlotBoss -= OnSelectSlotBoss;
    }
    private void OnSelectSlotBoss(BossSlot bossSlot)
    {
        foreach (var item in bossSlotList)
        {
            if(item != bossSlot)
            {
                item.selected = false;
            }
        }
    }

    void Init()
    {
        List<BossPackedConfig> bossPackedConfigList =
            managerRoot.ManagerRootConfig.availableBossConfig.bossPackedConfigList;

        for (int i = 0; i < bossPackedConfigList.Count; i++)
        {
            BossPackedConfig bossPackedConfig = bossPackedConfigList[i];
            BossSlot bossSlot = Instantiate(bossSlotPrefab, transform);
            bossSlot.SetIndex(i);
            bossSlot.bossPackedConfig = bossPackedConfig;

            bossSlotList.Add(bossSlot);
        }
    }
}
