using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档管理器类，负责游戏数据的保存和加载功能
/// </summary>
public class SaveGameManager : MonoBehaviour
{
    /// <summary>
    /// 静态存档数据实例，用于存储游戏状态信息
    /// </summary>
    public static SaveData Data;

    /// <summary>
    /// Unity生命周期函数，在对象启用时执行初始化操作
    /// 订阅加载游戏事件并初始化存档数据
    /// </summary>
    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        Data = new SaveData();
    }

    /// <summary>
    /// 保存游戏数据到存储介质
    /// 将当前内存中的数据序列化并持久化存储
    /// </summary>
    public static void SaveData()
    {
        var saveData = Data;
        SaveLoad.Save(saveData);
        Debug.Log("Save key has been pressed...");
    }
    
    /// <summary>
    /// 加载游戏数据回调函数
    /// 当触发加载事件时，将传入的存档数据替换当前运行时数据
    /// </summary>
    /// <param name="saveData">从存储介质中读取的存档数据对象</param>
    private static void LoadGame(SaveData saveData)
    {
        Data = saveData;
    }

    /// <summary>
    /// 尝试从存储介质加载数据
    /// 触发加载流程，实际的数据赋值通过事件回调完成
    /// </summary>
    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
    
    /// <summary>
    /// Unity生命周期函数，在对象销毁时执行清理操作
    /// 取消订阅加载游戏事件，防止内存泄漏
    /// </summary>
    private void OnDestroy()
    {
        SaveLoad.OnLoadGame -= LoadGame;
    }
}