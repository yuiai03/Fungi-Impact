using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;

    private void Awake()
    {
        EventManager.onCameraChangeTarget += OnCameraChangeTarget;
    }
    private void OnDestroy()
    {
        EventManager.onCameraChangeTarget -= OnCameraChangeTarget;
    }
    private void FixedUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    void OnCameraChangeTarget(Transform target)
    {
        this.target = target;
    }
}
