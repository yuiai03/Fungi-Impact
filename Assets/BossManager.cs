using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private BossInfoReader bossPrefab;
    public List<Transform> spawnPointList = new List<Transform>();
    public BossData currentBossData = new BossData();


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
        BossNameType bossNameType = managerRoot.actionBossNameType;
        AvailableBossConfig availableBossConfig = managerRoot.ManagerRootConfig.availableBossConfig;

        BossPackedConfig bossPackedConfig = availableBossConfig.GetBossPackedConfigByNameType(bossNameType);
        
        BossData bossData = new BossData();
        bossData.bossConfig = bossPackedConfig.config;
        bossData.bossStats = bossPackedConfig.stats;

        bossData.lv = bossData.bossStats.lv;
        bossData.maxHealth = bossData.bossStats.maxHealth;
        bossData.health = bossData.bossStats.maxHealth;
        bossData.damage = bossData.bossStats.damage;
        bossData.moveSpeed = bossData.bossStats.moveSpeed;

        currentBossData = bossData;
    }
}
