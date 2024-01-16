using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossInfoReader BossInfo { get => bossInfo; }
    private BossInfoReader bossInfo;
    private BossCollider bossCollider;
    private BossAttack bossAttack;
    private BossHealth bossHealth;

    private void Awake()
    {
        bossAttack = GetComponent<BossAttack>();
        bossCollider = GetComponent<BossCollider>();
        bossInfo = GetComponent<BossInfoReader>();
        bossHealth = GetComponent<BossHealth>();
    }

    public Transform Target()
    {
        if (bossCollider.ColliderList().Length == 0) return null;
        
        return bossCollider.ColliderList()[0].transform;

    }
}
