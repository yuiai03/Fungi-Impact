using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fungus Stat", menuName = "Stats/Fungus")]
public class FungusStats : ScriptableObject
{
    public int fungusMaxHealth = 10000;
    public int fungusDamage = 10000;
    public int fungusLevel = 1;
    public float fungusSpeed = 3f;
}
