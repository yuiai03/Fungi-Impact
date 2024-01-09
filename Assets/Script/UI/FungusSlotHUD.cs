using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FungusSlotHUD : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider damageSlider;
    [SerializeField] private Slider slotInteractRecoveryBar;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI inputSlotText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI slotInteractRecoveryText;

    [Header("Image")]
    [SerializeField] private Image avatarImage;

    [Header("Obj")]
    [SerializeField] private GameObject slotInteractRecoveryObj;

    private Image choseBgImage;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        choseBgImage = GetComponentInChildren<Image>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }
    private void Start()
    {
        SetSlotInteractRecoverySliderInit(0, GameConfig.switchSlotRecoveryTime);
    }
    public void SetHealthSliderInit(float minValue, float maxValue)
    {
        healthSlider.minValue = minValue;
        healthSlider.maxValue = maxValue;
    }
    public void SetDamageSliderInit(float minValue, float maxValue)
    {
        damageSlider.minValue = minValue;
        damageSlider.maxValue = maxValue;
    }
    public void SetSlotInteractRecoverySliderInit(float minValue, float maxValue)
    {
        slotInteractRecoveryBar.minValue = minValue;
        slotInteractRecoveryBar.maxValue = maxValue;
    }
    public void SetHealthSlider(float value) => healthSlider.value = value;
    public void SetSlotInteractRecoverySlider(float value) => slotInteractRecoveryBar.value = value;
    public void SetDamageSlider(float value) => damageSlider.value = value;
    
    public void SetName(string name) => nameText.text = name;

    public void SetFade(float value) => canvasGroup.alpha = value;

    public void SetSlotInteractRecoveryState(bool state) => slotInteractRecoveryObj.SetActive(state);

    public void SetAvatar(Sprite sprite) => avatarImage.sprite = sprite;
    
    public void SetInputSlot(KeyCode keyCode)
    {
        inputSlotText.text = keyCode.ToString().Substring(keyCode.ToString().Length - 1);
    }
    public void SetChoseBgColor(Color color, float alpha)
    {
        Color newColor = color;
        newColor.a = alpha;
        choseBgImage.color = newColor;
    }
    public Color GetChoseBgColor()
    {
        return choseBgImage.color;
    }
    public void SetSlotInteractRecoveryTime(float value)
    {
        slotInteractRecoveryText.text = value.ToString("F1");
    }
}
