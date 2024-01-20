using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossInfoReader BossInfo { get; private set; }
    public BossCollider BossCollider { get; private set; }
    public BossAttack BossAttack{ get; private set; }
    public BossHealth BossHealth{ get; private set; }

    private void Awake()
    {
        BossAttack = GetComponent<BossAttack>();
        BossCollider = GetComponent<BossCollider>();
        BossInfo = GetComponent<BossInfoReader>();
        BossHealth = GetComponent<BossHealth>();
    }

    public Transform Target()
    {
        if (BossCollider.ColliderList().Length == 0) return null;
        
        return BossCollider.ColliderList()[0].transform;

    }
}
