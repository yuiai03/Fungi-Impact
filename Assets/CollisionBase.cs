using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBase : MonoBehaviour
{
    [SerializeField] private Collider2D[] colliderList;
    [SerializeField] protected float radius;
    [SerializeField] protected LayerMask layerCheck;

    private void Update()
    {
        colliderList = Physics2D.OverlapCircleAll(transform.position, radius, layerCheck);
    }
    protected bool IsCollisionLayer()
    {
        return colliderList.Length > 0;
    }
    protected void SetRadius(float value)
    {
        radius = value;
    }
    public Collider2D[] ColliderList()
    {
        return colliderList;
    }
    private void OnDrawGizmos()
    {
        if (colliderList.Length == 0) return;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
