using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

    [SerializeField] private Building spawnedBuilding;

    private Building _targetBuilding;

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
        if (IsRayHittingSomething(deleteModeLayerMask, out RaycastHit hitInfo))
        {
            var detectedBuilding = hitInfo.collider.gameObject.GetComponentInParent<Building>();
        
            if (detectedBuilding == null) return;
        
            if (_targetBuilding == null) _targetBuilding = detectedBuilding;

            // 切换目标建筑时取消之前的目标删除标记
            if (detectedBuilding != _targetBuilding && _targetBuilding.flaggedForDelete)
            {
                _targetBuilding.RemoveDeleteFlag();
                _targetBuilding = detectedBuilding;
            }

            // 当前目标未被标记为删除时进行标记
            if (detectedBuilding == _targetBuilding && !_targetBuilding.flaggedForDelete)
            {
                _targetBuilding.FlagForDelete(buildingMatNegative);
            }
        
            // 鼠标左键点击时销毁击中的物体
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Destroy(_targetBuilding.gameObject);
                _targetBuilding = null;
            }
        }
        else
        { 
            // 射线未命中任何对象且有已选中的目标时移除其删除标记
            if (_targetBuilding != null && _targetBuilding.flaggedForDelete)
            {
                _targetBuilding.RemoveDeleteFlag();
                _targetBuilding = null;
            }
        }
    }

    /// <summary>
    /// 建造模式逻辑，处理物体放置操作
    /// </summary>
    private void BuildModeLogic()
    {
        // 若处于建造模式但仍有待删除的目标，则清除该状态
        if (_targetBuilding != null && _targetBuilding.flaggedForDelete)
        {
            _targetBuilding.RemoveDeleteFlag();
            _targetBuilding = null;
        }

        // 检查是否有待放置的物体
        if (spawnedBuilding == null) return;
        
        // 检测是否击中可建造表面
        if (!IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            // 未命中有效位置时显示无效材料效果
            spawnedBuilding.UpdateMaterial(buildingMatNegative);
        }
        else
        {
            // 命中有效位置时应用正常材料并调整到网格对齐位置
            spawnedBuilding.UpdateMaterial(buildingMatPositive);
            var gridPosition = WorldGrid.GridPositionFromWorldPoint3D(hitInfo.point, 1f);
            spawnedBuilding.transform.position = gridPosition;
            
            // 鼠标左键点击时在击中位置实例化物体
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Building placedBuilding = Instantiate(spawnedBuilding, gridPosition, Quaternion.identity);
                placedBuilding.PlaceBuilding();
            }
        }
    }
}
