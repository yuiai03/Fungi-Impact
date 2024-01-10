using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "New Fungus Config", menuName = "Config/Fungus")]
public class FungusConfig : ScriptableObject
{
    [Header("Info")]
    public string fungusName;
    public string fungusDescription;

    public Sprite fungusModelSprite;
    public Sprite fungusAvatar;

    public Color fungusColor;

    public Gradient gradientBullet;
    public Gradient gradientParticle;

    public AnimatorController animatorController;
    public Elemental fungusElemental;

}
[System.Serializable]
public class Elemental
{
    public ElementalType elementalType;
    public Sprite elementalSprite;
}
