using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏存档和读取管理类，提供游戏数据的保存和加载功能
/// </summary>
public static class SaveLoad
{
    /// <summary>
    /// 游戏保存时触发的事件回调
    /// </summary>
    public static UnityAction OnSaveGame;
    
    /// <summary>
    /// 游戏加载时触发的事件回调
    /// </summary>
    public static UnityAction<SaveData> OnLoadGame;

    /// <summary>
    /// 存档文件目录路径
    /// </summary>
    public static string Directory = "/SaveData/";
    
    /// <summary>
    /// 存档文件名
    /// </summary>
    public static string FileName = "SaveGame.sav";

    /// <summary>
    /// 保存游戏数据到本地文件
    /// </summary>
    /// <param name="data">需要保存的游戏数据对象</param>
    public static void Save(SaveData data)
    {
        // 触发保存事件回调
        OnSaveGame?.Invoke();
        
        // 构建完整的存档目录路径
        string dir = Application.persistentDataPath + Directory;
        
        // 检查并创建存档目录
        if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
        
        // 将数据序列化为JSON格式并写入文件
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + FileName, json);
        
        Debug.Log("Saving game...");
    }

    /// <summary>
    /// 从本地文件加载游戏数据
    /// </summary>
    public static void Load()
    {
        // 构建完整的存档文件路径
        string fullPath = Application.persistentDataPath + Directory + FileName;
        SaveData data = new SaveData();

        // 检查存档文件是否存在
        if (File.Exists(fullPath))
        {
            // 读取文件内容并反序列化为游戏数据对象
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);
            
            // 触发加载事件回调
            OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("Save file does not exist...");
        } 
    }
}