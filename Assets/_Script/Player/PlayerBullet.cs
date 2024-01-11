using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private BulletExplosion bulletExplosionPrefab;

    public Transform target;
    public Vector3 direction;

    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private Gradient gradientParticle;
    private Gradient gradientBullet;

    private PlayerInfoReader playerInfo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        trailRenderer.Clear();
    }
    public void GetPlayerInfo(PlayerInfoReader playerInfo)
    {
        this.playerInfo = playerInfo;
    }
    public void GetConfig(Color color, Gradient gParticle, Gradient gBullet)
    {
        gradientBullet = gBullet;
        gradientParticle = gParticle;

        spriteRenderer.color = color;
        trailRenderer.colorGradient = gBullet;
    }
    public void MoveToTarget()
    {
        if (target != null)
        {
            Vector3 targetPos = target.transform.position;
            direction = (targetPos - transform.position).normalized;
        }
        rb2d.velocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") || collision.CompareTag("PlayerBullet") || collision.CompareTag("BossBullet")) return;

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossController>();
            Vector3 collisionPos = collision.transform.position;

            collision.GetComponent<BossHealth>().TakeDamage(playerInfo.PlayerData.damage);

            TextPopUp textPopUp = PoolManager.instance.SpawnObj(PoolManager.instance.textPopUpPrefab, collisionPos, PoolType.TextPopUp);
            textPopUp.SetPopUpDamage(playerInfo.PlayerData.damage, playerInfo.PlayerData.fungusConfig.fungusColor);

        }
        gameObject.SetActive(false);
        
        BulletExplosion bulletExplosion = PoolManager.instance.SpawnObj(bulletExplosionPrefab, transform.position, PoolType.PlayerExplosion);
        bulletExplosion.GetParticleGradient(gradientParticle, gradientBullet);
    }
}
