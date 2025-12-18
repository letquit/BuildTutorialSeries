using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 建造工具类，用于处理建造和删除物体的逻辑
/// </summary>
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

    /// <summary>
    /// 初始化组件引用
    /// </summary>
    private void Start()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// 每帧更新，处理输入和模式切换逻辑
    /// </summary>
    private void Update()
    {
        // Q键切换删除模式
        if (Keyboard.current.qKey.wasPressedThisFrame) _deleteModeEnabled = !_deleteModeEnabled;
        
        // 根据当前模式执行相应逻辑
        if (_deleteModeEnabled)
            DeleteModeLogic();
        else
            BuildModeLogic();
    }

    /// <summary>
    /// 发射射线检测是否击中指定图层的物体
    /// </summary>
    /// <param name="layerMask">要检测的图层掩码</param>
    /// <param name="hitInfo">射线击中信息的输出参数</param>
    /// <returns>如果射线击中物体则返回true，否则返回false</returns>
    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, _camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, layerMask);
    }

    /// <summary>
    /// 删除模式逻辑，处理物体删除操作
    /// </summary>
    private void DeleteModeLogic()
    {
        // 检测是否击中可删除物体
        if (!IsRayHittingSomething(deleteModeLayerMask, out RaycastHit hitInfo)) return;
        
        // 鼠标左键点击时销毁击中的物体
        if (Mouse.current.leftButton.wasPressedThisFrame) Destroy(hitInfo.collider.gameObject);
    }

    /// <summary>
    /// 建造模式逻辑，处理物体放置操作
    /// </summary>
    private void BuildModeLogic()
    {
        // 检查是否有待放置的物体
        if (gameObjectToPosition == null) return;
        
        // 检测是否击中可建造表面
        if (!IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo)) return;
        
        // 将待放置物体定位到击中点
        gameObjectToPosition.transform.position = hitInfo.point;

        // 鼠标左键点击时在击中位置实例化物体
        if (Mouse.current.leftButton.wasPressedThisFrame)
            Instantiate(gameObjectToPosition, hitInfo.point, Quaternion.identity);
    }
}