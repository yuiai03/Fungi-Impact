using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum SkillType
{
    NA,
    ES,
    EB
}
public class FungusSkill : MonoBehaviour, ISkill
{

    [SerializeField] private FungusBullet bulletPrefab;

    private FungusController fungusController;
    private CameraCollider cameraCollider => CameraCollider.Instance;

    private void Awake()
    {
        fungusController = GetComponent<FungusController>();
    }

    public void NA_Skill()
    {
        FungusBullet bullet;
        bullet = PoolManager.Instance.SpawnObj(bulletPrefab, transform.position, PoolType.FungusBullet);
        if (bullet != null)
        {
            FungusInfoReader fungusInfo = fungusController.FungusInfo;
            FungusConfig config =  fungusInfo.FungusData.fungusConfig;

            bullet.Target = cameraCollider.GetTargetTransform();
            bullet.Direction = fungusController.DirectionAttackWithOutTarget();
            bullet.GetConfig(config.fungusColor, config.gradientParticle, config.gradientBullet);
            bullet.MoveToTarget();
            bullet.GetPlayerInfo(fungusInfo);
        }
    }
    public void ES_Skill()
    {
    }
    public void EB_Skill()
    {
    }

}
