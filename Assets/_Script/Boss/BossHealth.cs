using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float countChangeDamageSlider;
    [SerializeField] private int takingDamage;
    private BossInfoReader bossInfo;
    private void Awake()
    {
        bossInfo = GetComponent<BossInfoReader>();

    }
    private void OnDestroy()
    {
    }
    private void Update()
    {
        UpdateCurrentDamageSlider();
    }
    public void TakeDamage(int value)
    {
        bossInfo.BossData.health -= value;

        if (bossInfo.BossData.health <= 0) bossInfo.BossData.health = 0;

        bossInfo.currentStatusHUD.SetCurrentHealthSlider(bossInfo.BossData.health);
        bossInfo.currentStatusHUD.SetHealthText(bossInfo.BossData.health, bossInfo.BossData.maxHealth);
        takingDamage = value;
    }
    void UpdateCurrentDamageSlider()
    {
        float healthValue = bossInfo.currentStatusHUD.currentHealthSlider.value;
        float damageValue = bossInfo.currentStatusHUD.currentDamageSlider.value;


        if (takingDamage == 0)
        {
            countChangeDamageSlider = 0;
            return;
        }

        if (Mathf.RoundToInt(healthValue) > Mathf.RoundToInt(damageValue))
        {
            ChangeCurrentDamageValue(true);
        }
        else if (Mathf.RoundToInt(healthValue) < Mathf.RoundToInt(damageValue))
        {
            ChangeCurrentDamageValue(false);
        }
        else
        {
            takingDamage = 0;
        }
    }
    void ChangeCurrentDamageValue(bool increase)
    {
        countChangeDamageSlider += Time.deltaTime;
        if (countChangeDamageSlider >= GameConfig.damageSliderChangeWaitTime)
        {
            if (increase)
            {
                bossInfo.currentStatusHUD.currentDamageSlider.value = bossInfo.BossData.health;
            }
            else
            {
                bossInfo.currentStatusHUD.currentDamageSlider.value -= takingDamage * 2 * Time.deltaTime;
            }
        }
    }
}
