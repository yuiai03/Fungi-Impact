using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    private ManagerRoot managerRoot => ManagerRoot.instance;
    private void Awake()
    {
        homeButton.onClick.AddListener(OnHomeClick);
    }

    private void OnHomeClick()
    {
        managerRoot.TransitionToScene(managerRoot.ManagerRootConfig.home);
    }

}
