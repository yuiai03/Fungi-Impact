using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class FungusBullet : MonoBehaviour
{
    #region Variables
    [SerializeField] private BulletExplosion bulletExplosionPrefab;

    [SerializeField] private float speed = 10;
    public Transform Target { get; set; }
    public Vector3 Direction { get; set; }

    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private Gradient gradientParticle;
    private Gradient gradientBullet;
    private TriggerDetection triggerDetection;

    private FungusInfoReader fungusInfo;
    private PoolManager poolManager => PoolManager.Instance;
    #endregion
    private void Awake()
    {
        triggerDetection = GetComponent<TriggerDetection>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        _ListenEvents();
    }

    void _ListenEvents()
    {
        triggerDetection.detectionEnterEvent.AddListener((GameObject obj) => TriggerDetection(obj));

    }
    private void OnEnable()
    {
        trailRenderer.Clear();
    }

    #region Get Set Reference
    public void GetPlayerInfo(FungusInfoReader playerInfo)
    {
        this.fungusInfo = playerInfo;
    }
    public void GetConfig(Color color, Gradient gParticle, Gradient gBullet)
    {
        gradientBullet = gBullet;
        gradientParticle = gParticle;

        spriteRenderer.color = color;
        trailRenderer.colorGradient = gBullet;
    }
    #endregion

    public void MoveToTarget()
    {
        if (Target != null)
        {
            Vector3 targetPos = Target.transform.position;
            Direction = (targetPos - transform.position).normalized;
        }
        rb2d.velocity = Direction * speed;
    }


    public void TriggerDetection(GameObject @object)
    {
        if (@object.GetComponent<BossHealth>())
        {
            Vector3 collisionPos = @object.transform.position;

            @object.GetComponent<BossHealth>().TakeDamage(fungusInfo.FungusData.atk);

            TextPopUp textPopUp;
            textPopUp = poolManager.SpawnObj(poolManager.GetTextPopUp(), collisionPos, PoolType.TextPopUp);
            textPopUp.SetPopUpDamage(fungusInfo.FungusData.atk, fungusInfo.FungusData.fungusConfig.fungusColor);
        }

        gameObject.SetActive(false);

        BulletExplosion bulletExplosion;
        bulletExplosion = poolManager.SpawnObj(bulletExplosionPrefab, transform.position, PoolType.Fungusxplosion);
        bulletExplosion.GetParticleGradient(gradientParticle, gradientBullet);
    }
}
