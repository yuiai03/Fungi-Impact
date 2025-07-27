using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, GameConfig.scaleCardDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.05f, GameConfig.scaleCardDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, GameConfig.scaleCardDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, GameConfig.scaleCardDuration);

        AudioManager.Instance.PlayOnClickButton();
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Room")
        {
            AudioManager.Instance.PlayTheme();
        }
    }
}
