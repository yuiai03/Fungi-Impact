using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private BossInfoReader bossPrefab;
    public List<BossData> bossDataList = new List<BossData>();
    public List<Transform> spawnPointList = new List<Transform>();


    private ManagerRoot managerRoot => ManagerRoot.instance;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        SpawnBoss();
    }
    public void SpawnBoss()
    {
        var boss = Instantiate(bossPrefab, spawnPointList[0].position, bossPrefab.transform.rotation);
        boss.transform.SetParent(transform);
    }
    public void GetDataBoss()
    {
        AvailableBossConfig availableBossConfig = managerRoot.ManagerRootConfig.availableBossConfig;
    }
}
