using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 存档数据类，用于保存游戏中的建筑数据
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// 建筑存档数据列表
    /// </summary>
    public List<BuildingSaveData> buildingSaveData;

    /// <summary>
    /// SaveData类的构造函数
    /// 初始化建筑存档数据列表
    /// </summary>
    public SaveData()
    {
        buildingSaveData = new List<BuildingSaveData>();
    }
}