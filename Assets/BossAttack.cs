using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private BossBullet bulletPrefab;
    public Transform target;

    private BossInfoReader bossInfo;
    private void Awake()
    {
        bossInfo = GetComponent<BossInfoReader>();
        bossInfo.OnGetTarget += OnGetTarget;
    }
    private void OnDestroy()
    {
        bossInfo.OnGetTarget -= OnGetTarget;
    }
    private void OnGetTarget(Transform transform)
    {
        target = transform;
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            BossBullet bullet = PoolManager.instance.SpawnObj(bulletPrefab, transform.position, PoolType.BossBullet);
            bullet.target = target;
            bullet.MoveToTarget();
            bullet.GetBossInfo(bossInfo);
        }
    }
}
