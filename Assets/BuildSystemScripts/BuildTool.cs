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
    [SerializeField] private float rotateSnapAngle = 90f;
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
    private Quaternion _lastRotation;

    public BuildingData data;

    /// <summary>
    /// 初始化组件引用，并根据初始数据选择要放置的部件
    /// </summary>
    private void Start()
    {
        _camera = Camera.main;
        
        ChoosePart(data);
    }

    /// <summary>
    /// 根据传入的数据创建一个新的预览建筑对象。如果当前处于删除模式或已有预览对象，则先清理旧状态。
    /// </summary>
    /// <param name="bData">用于初始化新建筑的数据</param>
    private void ChoosePart(BuildingData bData)
    {
        // 如果当前处于删除模式，清除删除标记并重置相关状态
        if (_deleteModeEnabled)
        {
            if (_targetBuilding != null && _targetBuilding.flaggedForDelete) _targetBuilding.RemoveDeleteFlag();
            _targetBuilding = null;
            _deleteModeEnabled = false;
        }

        // 销毁已存在的预览建筑对象
        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding.gameObject);
            spawnedBuilding = null;
        }

        // 创建新的预览建筑对象并初始化
        var go = new GameObject
        {
            layer = defaultLayerInt,
            name = "Build Preview"
        };
        
        spawnedBuilding = go.AddComponent<Building>();
        spawnedBuilding.Init(bData);
        spawnedBuilding.transform.rotation = _lastRotation;
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
    /// 删除模式逻辑，处理物体删除操作。包括高亮显示可删除对象、标记待删对象以及响应鼠标点击进行实际销毁。
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
    /// 建造模式逻辑，处理物体放置操作。包括定位预览模型、旋转控制及最终放置确认。
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

        PositionBuildingPreview();
    }

    /// <summary>
    /// 控制预览建筑的位置与交互行为：更新材质以反映重叠状态、响应R键旋转、定位到网格点并响应鼠标左键完成放置。
    /// </summary>
    private void PositionBuildingPreview()
    {
        // 根据是否重叠来更新建筑预览的材质显示
        spawnedBuilding.UpdateMaterial(spawnedBuilding.isOverlapping ? buildingMatNegative : buildingMatPositive);
        
        // 处理建筑旋转逻辑：按下R键时按指定角度旋转建筑
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            spawnedBuilding.transform.Rotate(0, rotateSnapAngle, 0);
            _lastRotation = spawnedBuilding.transform.rotation;
        }
        
        // 处理建筑定位和放置逻辑
        if (IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            // 将世界坐标点转换为网格坐标并设置建筑位置
            var gridPosition = WorldGrid.GridPositionFromWorldPoint3D(hitInfo.point, 1f);
            spawnedBuilding.transform.position = gridPosition;
            
            // 处理建筑放置：当鼠标左键点击且建筑未重叠时完成放置
            if (Mouse.current.leftButton.wasPressedThisFrame && !spawnedBuilding.isOverlapping)
            {
                spawnedBuilding.PlaceBuilding();
                var dataCopy = spawnedBuilding.assignedData;
                spawnedBuilding = null;
                ChoosePart(dataCopy);
            }
        }
    }
}