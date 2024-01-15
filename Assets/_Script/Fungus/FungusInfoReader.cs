using UnityEditor.Animations;
using UnityEngine;

public class FungusInfoReader : MonoBehaviour
{

    [SerializeField] private SpriteRenderer model;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem switchFungusEffect;
    [SerializeField] private ParticleSystem shadowSwitchFungusEffect;
    
    [Header("Data")]
    [SerializeField] private FungusData fungusData;
    public FungusData FungusData { get => fungusData; }

    public void GetData(FungusData data)
    {
        fungusData = data;

        GetModel(fungusData.fungusConfig.fungusModelSprite, fungusData.fungusConfig.dissolveMaterial);

        GetAnimatorController(this.fungusData.fungusConfig.animatorController);

        GetParticleGradient(this.fungusData.fungusConfig.gradientParticle);

        switchFungusEffect.Play();
    }
    void GetParticleGradient(Gradient gradient)
    {
        var switchColor = switchFungusEffect.colorOverLifetime;
        switchColor.color = gradient;

        var shadowSwitchColor = shadowSwitchFungusEffect.colorOverLifetime;
        shadowSwitchColor.color = gradient;
    }
    void GetModel(Sprite sprite, Material material)
    {
        model.sprite = sprite;
        model.material = material;
    }
    void GetAnimatorController(AnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }
}
