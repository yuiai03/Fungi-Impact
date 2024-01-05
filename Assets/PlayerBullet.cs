using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;
    public Transform target;
    public Vector3 direction;
    [SerializeField] private BulletExplosion bulletExplosion;
    [SerializeField] private Color spriteColor;
    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
    }
    private void OnEnable()
    {
        spriteRenderer.color = spriteColor;
        trailRenderer.Clear();
    }
    private void OnDisable()
    {
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

        if (collision.CompareTag("Player") || collision.CompareTag("PlayerBullet")) return;

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>();
        }
        PoolManager.instance.SpawnObj(bulletExplosion, transform.position, PoolType.Explosion);
        gameObject.SetActive(false);
    }
}
