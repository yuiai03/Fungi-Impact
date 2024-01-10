using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossData
{
    public float maxHealth;
    public float health;

    public int damage;

    public int lv;

    public float moveSpeed;

    public BossConfig bossConfig;
    public BossStats bossStats;
}
