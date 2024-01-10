using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [Header("Prefab")]
    public TextPopUp textPopUpPrefab;

    [Header("Holder")]
    [SerializeField] private GameObject ObjectPoolHolder;

    [SerializeField] private GameObject playerBulletHolder;
    [SerializeField] private GameObject playerExplosionHolder;

    [SerializeField] private GameObject bossBulletHolder;
    [SerializeField] private GameObject bossExplosionHolder;

    [SerializeField] private GameObject textPopUpHolder;

    [Header("List Obj")]
    [SerializeField] private List<PlayerBullet> playerBulletList;
    [SerializeField] private List<BulletExplosion> playerExplosionList;

    [SerializeField] private List<BossBullet> bossBulletList;
    [SerializeField] private List<TextPopUp> textPopUpList;
    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetUpObjEmpties();
    }
    private void Update()
    {
        
    }
    void SetUpObjEmpties()
    {
        ObjectPoolHolder = new GameObject("ObjectPoolHolder");

        playerBulletHolder = new GameObject("BulletHolder");
        playerBulletHolder.transform.SetParent(ObjectPoolHolder.transform);

        playerExplosionHolder = new GameObject("ExplosionHolder");
        playerExplosionHolder.transform.SetParent(ObjectPoolHolder.transform);

        bossBulletHolder = new GameObject("BossBulletHolder");
        bossBulletHolder.transform.SetParent(ObjectPoolHolder.transform);

        bossExplosionHolder = new GameObject("BossExplosionHolder");
        bossExplosionHolder.transform.SetParent(ObjectPoolHolder.transform);

        textPopUpHolder = new GameObject("TextPopUpHolder");
        textPopUpHolder.transform.SetParent(ObjectPoolHolder.transform);
    }
    public GameObject GetObjHolder(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.PlayerBullet:
                return playerBulletHolder;
            case PoolType.PlayerExplosion:
                return playerExplosionHolder;
            case PoolType.BossBullet:
                return bossBulletHolder;
            case PoolType.BossExplosion:
                return null;
            case PoolType.TextPopUp:
                return textPopUpHolder;
            default:
                return null;
        }
    }
    public List<T> GetListObj<T>(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.PlayerBullet:
                return playerBulletList as List<T>;
            case PoolType.PlayerExplosion:
                return playerExplosionList as List<T>;
            case PoolType.BossBullet:
                return bossBulletList as List<T>;
            case PoolType.BossExplosion:
                return null;
            case PoolType.TextPopUp:
                return textPopUpList as List<T>;
            default:
                return null;
        }
    }
    public T SpawnObj<T>(T spawnComponent, Vector2 spawnPos, PoolType poolType = PoolType.None) where T : Component
    {
        List<T> objList = GetListObj<T>(poolType); //Get list obj holder
        T inactiveObj = objList.Find(p => !p.gameObject.activeSelf); //Find inactive obj

        if (inactiveObj == null) //Inactive obj not found
        {
            T spawnableObj = Instantiate(spawnComponent, spawnPos, Quaternion.identity);

            objList.Add(spawnableObj);

            GameObject holder = GetObjHolder(poolType); //Get obj holder
            if (holder != null) spawnableObj.transform.SetParent(holder.transform);

            return spawnableObj;
        }
        else
        {
            inactiveObj.gameObject.transform.position = spawnPos;
            inactiveObj.gameObject.SetActive(true);

            return inactiveObj;
        }
    }

}
