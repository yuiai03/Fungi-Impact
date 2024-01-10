using UnityEditor.Animations;
using UnityEngine;

public class PlayerInfoReader : MonoBehaviour
{

    [SerializeField] private SpriteRenderer model;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem switchFungusEffect;
    [SerializeField] private ParticleSystem shadowSwitchFungusEffect;
    
    [Header("HUD")]    
    public PlayerCurrentStatusHUD playerCurrentHUD;

    [Header("Data")]
    [SerializeField] private FungusData playerData;
    public FungusData PlayerData { get => playerData; }

    private void Awake()
    {
    }
    public void GetData(FungusData fungusData)
    {
        playerData = fungusData;
        GetModel(playerData.fungusConfig.fungusModelSprite);
        GetAnimatorController(playerData.fungusConfig.animatorController);

        GetParticleGradient(playerData.fungusConfig.gradientParticle);
        switchFungusEffect.Play();
    }
    void GetParticleGradient(Gradient gradient)
    {
        var switchColor = switchFungusEffect.colorOverLifetime;
        switchColor.color = gradient;

        var shadowSwitchColor = shadowSwitchFungusEffect.colorOverLifetime;
        shadowSwitchColor.color = gradient;
    }
    void GetModel(Sprite sprite) => model.sprite = sprite;
    void GetAnimatorController(AnimatorController animatorController) => animator.runtimeAnimatorController = animatorController;
}
