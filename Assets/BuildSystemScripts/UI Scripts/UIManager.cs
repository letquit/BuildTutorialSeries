using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// UI管理器类，负责管理游戏中的用户界面显示和交互
/// </summary>
public class UIManager : MonoBehaviour
{
    public BuildingPanelUI buildPanel;

    /// <summary>
    /// 初始化UI管理器，在游戏开始时隐藏建筑面板
    /// </summary>
    private void Start()
    {
        buildPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 每帧检查输入，处理Tab键按下事件来切换建筑面板的显示状态
    /// </summary>
    private void Update()
    {
        // 检测Tab键是否在当前帧被按下
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            // 切换建筑面板的激活状态
            buildPanel.gameObject.SetActive(!buildPanel.gameObject.activeInHierarchy);
        }
    }
}