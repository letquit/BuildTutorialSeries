using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建筑部件UI控制器类，负责处理建筑部件按钮的显示和交互逻辑
/// </summary>
public class BuildingPartUI : MonoBehaviour
{
    private Button _button;
    private BuildingData _assignedData;
    private BuildingPanelUI _parentDisplay;

    /// <summary>
    /// 在对象被唤醒时执行初始化操作，获取子物体中的按钮组件并添加点击事件监听器
    /// </summary>
    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// 初始化建筑部件UI界面
    /// </summary>
    /// <param name="assignedData">要显示的建筑数据</param>
    /// <param name="parentDisplay">父级显示面板引用</param>
    public void Init(BuildingData assignedData, BuildingPanelUI parentDisplay)
    {
        _assignedData = assignedData;
        // 设置按钮图片为建筑数据中的图标
        _button.GetComponent<Image>().sprite = _assignedData.icon;
        _parentDisplay = parentDisplay;
    }
    
    /// <summary>
    /// 按钮点击事件处理函数，通知父级面板当前选中的建筑数据
    /// </summary>
    private void OnButtonClick()
    {
        _parentDisplay.OnClick(_assignedData);
    }
}