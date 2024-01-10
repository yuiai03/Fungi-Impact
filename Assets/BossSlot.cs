using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossSlot : MonoBehaviour
{
    public bool selected;
    [SerializeField] private TextMeshProUGUI index;
    public BossPackedConfig bossPackedConfig;
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnSlotClick);
    }

    private void OnSlotClick()
    {
        selected = true;
        EventManager.ActionOnSelectSlotBoss(this);
    }

    public void SetIndex(int value) => index.text = value.ToString();
    private void Update()
    {
    }
}
