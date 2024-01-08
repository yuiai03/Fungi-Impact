using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Fungus Config", menuName = "Config/Fungus")]
public class FungusConfig : ScriptableObject
{
    [Header("Info")]
    public string fungusName;
    public string fungusDescription;
    public Sprite fungusAvatar;
    public Color fungusColor;

    public Elemental fungusElemental;

}
public enum ElementalType
{
    Hydro,
    Pyro,
    Geo,
    Cyro,
    Anemo,
    Electro,
    Dendro
}
[System.Serializable]
public class Elemental
{
    public ElementalType elementalType;
    public Sprite elementalSprite;
}
