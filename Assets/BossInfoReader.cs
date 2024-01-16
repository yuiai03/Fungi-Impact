using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoReader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer model;


    public BossCurrentStatusHUD currentStatusHUD;
    public BossData BossData { get => bossData; }
    [SerializeField] private BossData bossData;


    public void GetData(BossData bossData)
    {
        this.bossData = bossData;

        GetModel(bossData.bossConfig.bossModelSprite);
    }

    public void GetCurrentStatusHUD(BossCurrentStatusHUD statusHUD)
    {
        currentStatusHUD = statusHUD;

        GetHUDInit();

    }

    void GetModel(Sprite sprite) => model.sprite = sprite;
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
