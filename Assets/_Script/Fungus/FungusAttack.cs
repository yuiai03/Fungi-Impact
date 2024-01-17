using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class FungusAttack : MonoBehaviour
{
    private FungusInfoReader fungusInfo;
    private FungusController fungusController;
    [SerializeField] private FungusBullet bulletPrefab;
    private CameraCollider cameraCollider => CameraCollider.instance;

    private void Awake()
    {

        fungusInfo = GetComponent<FungusInfoReader>();
        fungusController = GetComponent<FungusController>();
    }

    public void AttackAction()
    {
        FungusBullet bullet = PoolManager.Instance.SpawnObj(bulletPrefab, transform.position, PoolType.FungusBullet);
        if (bullet != null)
        {
            var config = fungusInfo.FungusData.fungusConfig;
            bullet.target = cameraCollider.GetTargetTransform();
            bullet.direction = fungusController.DirectionAttackWithOutTarget();
            bullet.GetConfig(config.fungusColor, config.gradientParticle, config.gradientBullet);
            bullet.MoveToTarget();
            bullet.GetPlayerInfo(fungusInfo);
        }
    }
}
