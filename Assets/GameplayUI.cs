using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Button returnButton;

    private ManagerRoot managerRoot => ManagerRoot.Instance;

    private void Awake()
    {
        returnButton.onClick.AddListener(OnReturnClick);
    }
    private void OnReturnClick()
    {
        managerRoot.TransitionToScene(managerRoot.ManagerRootConfig.home);

    }
}
