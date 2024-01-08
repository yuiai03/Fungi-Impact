using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ManagerRootConfig
{
    public string mainMenu = "Main Menu";
    public string teamSetup = "TeamSetup";
    public string home = "Home";
    public string room = "Room";
    public AvailableFungiConfig availableFungiConfig;

}
public class ManagerRoot : MonoBehaviour
{
    public List<string> actionFungusNameList = new List<string>();
    [SerializeField] private TransitionController transitionController;
    public ManagerRootConfig ManagerRootConfig { get => managerRootConfig; }
    [SerializeField] private ManagerRootConfig managerRootConfig;


    public static ManagerRoot instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GetNameFungusPicked(List<FungusSlot> fungusSlotList)
    {
        foreach(var fungusSlot in fungusSlotList)
        {
            actionFungusNameList.Add(fungusSlot.FungusPackedConfig.config.fungusName);
        }
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(ProgressTransitionToScene(sceneName));
    }
    IEnumerator ProgressTransitionToScene(string sceneName)
    {
        Debug.Log("Scene Loading");
        yield return new WaitForSeconds(0.5f);
        var asyncOperator = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperator.isDone)
        {
            float process = Mathf.Clamp01(asyncOperator.progress / 0.9f);
            yield return null;
        }
    }
}
