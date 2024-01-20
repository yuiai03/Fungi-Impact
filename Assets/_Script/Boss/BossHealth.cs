using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class BossHealth : MonoBehaviour
{
    private BossData bossData;

    public Action<int> OnTakeDamageEvent;
    public Action OnDiedEvent;

    private void Awake()
    {
        EventManager.onSpawnBossInit += OnSpawnBossInit;

    }
    private void OnDestroy()
    {
        EventManager.onSpawnBossInit -= OnSpawnBossInit;
    }


    void OnSpawnBossInit(BossInfoReader info)
    {
        bossData = info.BossData;
    }

    public void TakeDamage(int value)
    {
        bossData.health -= value;

        if (bossData.health <= 0) bossData.health = 0;

        var takingDamage = value;

        OnTakeDamageEvent?.Invoke(takingDamage);
        if(bossData.health <= 0)
        {
            bossData.health = 0;
            OnDiedEvent?.Invoke();
        }
    }
 
}
