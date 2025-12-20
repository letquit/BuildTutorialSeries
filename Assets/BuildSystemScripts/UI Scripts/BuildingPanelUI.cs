using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 建筑面板UI控制器类，负责处理建筑部件选择的UI交互
/// </summary>
public class BuildingPanelUI : MonoBehaviour
{
    public BuildingSideUI sideUI;
    public static UnityAction<BuildingData> OnPartChosen;

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
}