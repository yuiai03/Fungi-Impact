using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoReader : MonoBehaviour
{
    public Transform target;
    [SerializeField] private SpriteRenderer model;

    public Action<Transform> OnGetTarget;

    public BossCurrentStatusHUD currentStatusHUD;
    public BossData BossData { get => bossData; }
    [SerializeField] private BossData bossData;


    public void GetData(BossData bossData, BossCurrentStatusHUD statusHUD, PlayerInfoReader playerInfo)
    {
        this.bossData = bossData;
        currentStatusHUD = statusHUD;

        GetTarget(playerInfo.transform);

        GetModel(bossData.bossConfig.bossModelSprite);

        GetHUDInit();
    }
    void GetModel(Sprite sprite) => model.sprite = sprite;
    void GetTarget(Transform target)
    {
        this.target = target;
        OnGetTarget?.Invoke(target);
    }
    void GetHUDInit()
    {
        currentStatusHUD.SetCurrentHealthSliderInit(0, bossData.maxHealth);
        currentStatusHUD.SetCurrentDamageSliderInit(0, bossData.maxHealth);

        currentStatusHUD.SetCurrentHealthSlider(bossData.health);
        currentStatusHUD.SetCurrentDamageSlider(bossData.health);

        currentStatusHUD.SetHealthText(bossData.health, bossData.maxHealth);
        currentStatusHUD.SetLvText(bossData.lv);
    }
}
