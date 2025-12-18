using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModeLayerMask;
    [SerializeField] private LayerMask deleteModeLayerMask;
    [SerializeField] private int defaultLayerInt = 8;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatNegative;

    private bool _deleteModeEnabled;
    
    private Camera _camera;

    public GameObject gameObjectToPosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (gameObjectToPosition is null || !IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo)) return;
        
        gameObjectToPosition.transform.position = hitInfo.point;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            Instantiate(gameObjectToPosition, hitInfo.point, Quaternion.identity);
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, _camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, layerMask);
    }
}