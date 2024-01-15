using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private BossBullet bulletPrefab;

    private BossInfoReader bossInfo;
    private void Awake()
    {
        bossInfo = GetComponent<BossInfoReader>();
    }
    private void OnDestroy()
    {
    }
    private void Start()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            while (bossInfo.targetInfo == null) yield return null;

            BossBullet bullet = PoolManager.Instance.SpawnObj(bulletPrefab, transform.position, PoolType.BossBullet);
            bullet.target = bossInfo.targetInfo.transform;
            bullet.MoveToTarget();
            bullet.GetBossInfo(bossInfo);
        }
    }
}
