using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerConfigReader playerConfig;
    private PlayerController playerController;
    [SerializeField] private PlayerBullet bulletPrefab;
    private CameraCollider cameraCollider => CameraCollider.instance;

    private void Awake()
    {

        playerConfig = GetComponent<PlayerConfigReader>();
        playerController = GetComponent<PlayerController>();
    }

    public void AttackAction()
    {
        PlayerBullet bullet = PoolManager.instance.SpawnObj<PlayerBullet>(bulletPrefab, transform.position, PoolType.Bullet);
        if (bullet != null)
        {
            bullet.target = cameraCollider.GetTargetTransform();
            bullet.direction = playerController.SetDirectionAttackWithOutTarget();
            bullet.MoveToTarget();
            bullet.GetConfig(playerConfig.fungusConfig.fungusColor, playerConfig.fungusConfig.gradientBullet);
        }
    }
}
