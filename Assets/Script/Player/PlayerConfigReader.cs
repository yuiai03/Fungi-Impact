using UnityEditor.Animations;
using UnityEngine;

public class PlayerConfigReader : MonoBehaviour
{
    public FungusConfig fungusConfig;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem switchFungusEffect;
    [SerializeField] private ParticleSystem shadowSwitchFungusEffect;
    public void GetConfig(FungusConfig config)
    {
        fungusConfig = config;
        GetModel(config.fungusModelSprite);
        GetAnimatorController(config.animatorController);

        GetParticleGradient(config.gradientParticle);
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
