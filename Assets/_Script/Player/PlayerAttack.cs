using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInfoReader playerInfo;
    private PlayerController playerController;
    [SerializeField] private PlayerBullet bulletPrefab;
    private CameraCollider cameraCollider => CameraCollider.instance;

    private void Awake()
    {

        playerInfo = GetComponent<PlayerInfoReader>();
        playerController = GetComponent<PlayerController>();
    }

    public void AttackAction()
    {
        PlayerBullet bullet = PoolManager.instance.SpawnObj(bulletPrefab, transform.position, PoolType.PlayerBullet);
        if (bullet != null)
        {
            var config = playerInfo.PlayerData.fungusConfig;
            bullet.target = cameraCollider.GetTargetTransform();
            bullet.direction = playerController.SetDirectionAttackWithOutTarget();
            bullet.GetConfig(config.fungusColor, config.gradientParticle, config.gradientBullet);
            bullet.MoveToTarget();
            bullet.GetPlayerInfo(playerInfo);
        }
    }
}
