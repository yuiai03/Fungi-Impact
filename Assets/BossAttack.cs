using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private BossBullet bulletPrefab;

    private BossController bossController;
    private void Awake()
    {
        bossController = GetComponent<BossController>();
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

            while (bossController.Target() == null) yield return null;

            BossBullet bullet = PoolManager.Instance.SpawnObj(bulletPrefab, transform.position, PoolType.BossBullet);
            bullet.target = bossController.Target();
            bullet.MoveToTarget();
            bullet.GetBossInfo(bossController.BossInfo);
        }
    }
}
