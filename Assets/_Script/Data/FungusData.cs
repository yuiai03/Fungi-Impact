using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FungusData
{

    public float maxHealth;
    public float health;
    
    public int damage;
    public int lv;


    public float moveSpeed;

    public float dashTime;
    public float dashForce;
    public int dashStamina;

    public FungusConfig fungusConfig;
    public FungusStats fungusStats;

}
