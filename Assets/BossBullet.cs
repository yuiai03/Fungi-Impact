using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Transform target;
    public Vector2 direction;
    [SerializeField] private float speed = 1;
    [SerializeField] private int damage = 1000;

    private Rigidbody2D rb2d;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);


            Vector3 collisionPos = collision.transform.position;
            TextPopUp textPopUp = PoolManager.instance.SpawnObj(PoolManager.instance.textPopUpPrefab, collisionPos, PoolType.TextPopUp);
        }
        gameObject.SetActive(false);
    }
}
