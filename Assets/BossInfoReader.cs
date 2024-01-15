using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoReader : MonoBehaviour
{
    public FungusInfoReader targetInfo;
    [SerializeField] private SpriteRenderer model;


    public BossCurrentStatusHUD currentStatusHUD;
    public BossData BossData { get => bossData; }
    [SerializeField] private BossData bossData;


    public void GetData(BossData bossData, BossCurrentStatusHUD statusHUD)
    {
        this.bossData = bossData;
        currentStatusHUD = statusHUD;

        GetModel(bossData.bossConfig.bossModelSprite);

        GetHUDInit();
    }
    void GetModel(Sprite sprite) => model.sprite = sprite;
    public void GetTarget(FungusInfoReader fungusInfo)
    {
        targetInfo = fungusInfo;
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
