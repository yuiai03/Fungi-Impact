using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class TextPopUp : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float scale = 1.2f;
    public TextMeshPro popUpText;
    private Sequence sequence;
    private Tween scaleTween;
    private void Update()
    {
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        popUpText.alpha = 1f;
        popUpText.color = Color.white;

        scaleTween = transform.DOScale(scale, duration);

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(transform.position.y + 1f, duration));
        sequence.Append(popUpText.DOFade(0, duration));
        sequence.OnComplete(HideObj);
    }
    public void SetPopUpDamage(int value, Color color)
    {
        popUpText.text = value.ToString();
        popUpText.color = color;
    }
    void HideObj()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        sequence.Kill();
        scaleTween.Kill();
    }
}
