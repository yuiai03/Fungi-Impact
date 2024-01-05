using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    private GameObject _shadow;
    private SpriteRenderer shadowSprite;
    [SerializeField] private SpriteRenderer modelSprite; 
    [SerializeField] private Material shadowMaterial;

    private void Awake()
    {
        shadowSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = transform;

        shadowSprite.sprite = modelSprite.sprite;
        shadowSprite.material = shadowMaterial;
    }
}
