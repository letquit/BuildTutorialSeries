using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建筑物侧面UI显示控制类
/// 负责管理建筑物图标和名称的显示
/// </summary>
public class BuildingSideUI : MonoBehaviour
{
    public Image buildingImage;
    public TMP_Text buildingText;

    /// <summary>
    /// 初始化UI组件状态
    /// 将建筑图像隐藏并将文本清空
    /// </summary>
    private void Start()
    {
        buildingImage.sprite = null;
        buildingImage.color = Color.clear;
        buildingText.text = "";
    }

    /// <summary>
    /// 更新侧面显示面板的内容
    /// 根据传入的建筑数据设置图标和显示名称
    /// </summary>
    /// <param name="data">建筑数据对象，包含图标和显示名称信息</param>
    public void UpdateSideDisplay(BuildingData data)
    {
        // 设置建筑图标和文本显示
        buildingImage.sprite = data.icon;
        buildingImage.color = Color.white;
        buildingText.text = data.displayName;
    }
}