using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 建筑面板UI控制器类，负责处理建筑部件选择的UI交互
/// </summary>
public class BuildingPanelUI : MonoBehaviour
{
    public BuildingSideUI sideUI;
    public static UnityAction<BuildingData> OnPartChosen;

    public BuildingData[] knownBuildingParts;
    public BuildingPartUI buildingButtonPrefab;

    public GameObject itemWindow;

    /// <summary>
    /// 当建筑部件被点击时的回调函数
    /// 负责触发部件选择事件并更新侧边显示
    /// </summary>
    /// <param name="chosenData">被选中的建筑数据对象</param>
    public void OnClick(BuildingData chosenData)
    {
        // 触发部件选择事件，通知其他监听者有部件被选中
        OnPartChosen?.Invoke(chosenData);
        // 更新侧边UI显示选中的建筑数据
        sideUI.UpdateSideDisplay(chosenData);
    }

    /// <summary>
    /// 显示所有建筑部件按钮
    /// </summary>
    public void OnClickAllParts()
    {
        PopulateButtons();
    }

    /// <summary>
    /// 显示房间类型建筑部件按钮
    /// </summary>
    public void OnClickRoomParts()
    {
        PopulateButtons(PartType.Room);
    }

    /// <summary>
    /// 显示走廊类型建筑部件按钮
    /// </summary>
    public void OnClickCorridorParts()
    {
        PopulateButtons(PartType.Corridor);
    }
    
    /// <summary>
    /// 根据已知建筑部件数据填充按钮列表，默认显示全部部件
    /// </summary>
    public void PopulateButtons()
    {
        SpawnButtons(knownBuildingParts);
    }

    /// <summary>
    /// 根据指定部件类型筛选并填充对应的建筑部件按钮
    /// </summary>
    /// <param name="chosenPartType">要筛选显示的建筑部件类型</param>
    public void PopulateButtons(PartType chosenPartType)
    {
        var buildingPieces = knownBuildingParts.Where(p => p.partType == chosenPartType).ToArray();
        SpawnButtons(buildingPieces);
    }

    /// <summary>
    /// 实例化并初始化建筑部件按钮
    /// </summary>
    /// <param name="buttonData">用于创建按钮的数据数组</param>
    public void SpawnButtons(BuildingData[] buttonData)
    {
        ClearButtons();
        
        foreach (var data in buttonData)
        {
            var spawnedButton = Instantiate(buildingButtonPrefab, itemWindow.transform);
            spawnedButton.Init(data, this);
        }
    }

    /// <summary>
    /// 清除当前窗口内的所有按钮实例
    /// </summary>
    public void ClearButtons()
    {
        foreach (var button in itemWindow.transform.Cast<Transform>())
        {
            Destroy(button.gameObject);
        }
    }
}