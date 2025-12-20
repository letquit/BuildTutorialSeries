using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础管理器类，负责处理游戏对象的加载和管理
/// </summary>
public class BaseManager : MonoBehaviour
{
    public int defaultLayerInt;
    
    /// <summary>
    /// 当组件启用时注册事件监听器
    /// </summary>
    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadBase;
    }
    
    /// <summary>
    /// 当组件禁用时注销事件监听器
    /// </summary>
    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadBase;
    }

    /// <summary>
    /// 加载基础建筑数据
    /// </summary>
    /// <param name="saveData">包含建筑保存数据的对象</param>
    private void LoadBase(SaveData saveData)
    {
        // 遍历所有保存的建筑数据并重新创建建筑对象
        foreach (var building in saveData.buildingSaveData)
        {
            // 创建新的预览建筑对象并初始化
            var go = new GameObject
            {
                layer = defaultLayerInt,
                name = building.buildingName
            };
        
            var spawnedBuilding = go.AddComponent<Building>();
            spawnedBuilding.Init(building.assignedData, building);
            spawnedBuilding.transform.rotation = building.rotation;
            spawnedBuilding.transform.position = building.position;
            
            spawnedBuilding.PlaceBuilding();
        }
    }
}