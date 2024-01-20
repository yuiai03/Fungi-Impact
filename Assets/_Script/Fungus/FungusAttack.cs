using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class FungusAttack : MonoBehaviour
{
    private FungusInfoReader fungusInfo;
    private FungusController fungusController;

    [SerializeField] private FungusBullet bulletPrefab;
    private CameraCollider cameraCollider => CameraCollider.Instance;

    private void Awake()
    {

        fungusInfo = GetComponent<FungusInfoReader>();
        fungusController = GetComponent<FungusController>();
    }
}
