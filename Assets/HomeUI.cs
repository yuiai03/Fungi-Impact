using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button teamSetupButton;
    [SerializeField] private BossList bossList;
    private ManagerRoot managerRoot => ManagerRoot.instance;
    private void Awake()
    {
        returnButton.onClick.AddListener(OnReturnClick);
        teamSetupButton.onClick.AddListener(OnTeamSetupClick);
    }

    private void OnTeamSetupClick()
    {
        bool canChangeScene = false;
        foreach(var bossSlot in bossList.bossSlotList)
        {
            if (bossSlot.selected) canChangeScene = true;
        }

        if (canChangeScene)
        {
            managerRoot.TransitionToScene(managerRoot.ManagerRootConfig.teamSetup);
        }
    }

    private void OnReturnClick()
    {
        managerRoot.TransitionToScene(managerRoot.ManagerRootConfig.mainMenu);

    }
}
