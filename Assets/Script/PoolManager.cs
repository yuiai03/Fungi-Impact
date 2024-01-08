using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public enum PoolType{
    None,
    Bullet,
    Explosion,
}
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [SerializeField] private GameObject explosionHolder;
    [SerializeField] private GameObject ObjectPoolHolder;
    [SerializeField] private GameObject butlletHolder;

    public List<PlayerBullet> bulletList;
    public List<BulletExplosion> explosionList;
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

        butlletHolder = new GameObject("BulletHolder");
        butlletHolder.transform.SetParent(ObjectPoolHolder.transform);

        explosionHolder = new GameObject("ExplosionHolder");
        explosionHolder.transform.SetParent(ObjectPoolHolder.transform);
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
            if (holder != null)
            {
                spawnableObj.transform.SetParent(holder.transform);
            }

            return spawnableObj;
        }
        else
        {
            inactiveObj.gameObject.transform.position = spawnPos;
            inactiveObj.gameObject.SetActive(true);

            return inactiveObj;
        }
    }
    public GameObject GetObjHolder(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.Bullet:
                return butlletHolder;
            case PoolType.Explosion:
                return explosionHolder;
            default:
                return null;
        }
    }
    public List<T> GetListObj <T>(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.Bullet:
                return bulletList as List<T>;
            case PoolType.Explosion:
                return explosionList as List<T>;
            default:
                return null;
        }
    }
}
