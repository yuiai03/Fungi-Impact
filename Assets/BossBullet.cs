using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Transform target;
    public Vector2 direction;
    [SerializeField] private float speed = 1;

    private Rigidbody2D rb2d;
    private BossInfoReader bossInfo;
    private PoolManager poolManager => PoolManager.Instance;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void GetBossInfo(BossInfoReader bossInfo)
    {
        this.bossInfo = bossInfo;
    }
    public void MoveToTarget()
    {
        if (target == null) return;

        Vector3 targetPos = target.transform.position;
        direction = (targetPos - transform.position).normalized;
        rb2d.velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss") || collision.CompareTag("PlayerBullet") || collision.CompareTag("BossBullet")) return;

        if (collision.CompareTag("Player"))
        {
            FungusController fungusController = collision.GetComponent<FungusController>();
            if (fungusController.isDying) return;

            fungusController.GetFungusHealth().TakeDamage(bossInfo.BossData.damage);

            Vector3 collisionPos = collision.transform.position;
            TextPopUp textPopUp = poolManager.SpawnObj(poolManager.GetTextPopUp(), collisionPos, PoolType.TextPopUp);
            textPopUp.SetPopUpDamage(bossInfo.BossData.damage, bossInfo.BossData.bossConfig.bossColor);
        }
        gameObject.SetActive(false);
    }
}
