using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private ParticleSystem shadowEffect;
    [SerializeField] private ParticleSystem dropEffect;

    public void GetParticleGradient(Gradient explosionGradient, Gradient dropGradient)
    {
        var explosionColor = explosionEffect.colorOverLifetime;
        explosionColor.color = explosionGradient;

        var shadowColor = shadowEffect.colorOverLifetime;
        shadowColor.color = explosionGradient;

        var dropColor = dropEffect.colorOverLifetime;
        dropColor.color = dropGradient;

    }
    private void OnEnable()
    {
        StartCoroutine(SetupDeactivate());
    }
    IEnumerator SetupDeactivate()
    {
        yield return new WaitForSeconds(PlayerConfig.deactivateBulletExplosionTime);
        gameObject.SetActive(false);
    }
}
