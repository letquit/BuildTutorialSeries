using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建筑物类，用于管理建筑物的渲染材质和删除标记状态
/// </summary>
public class Building : MonoBehaviour
{
    private Renderer _renderer;
    private Material _defaultMaterial;

    private bool _flaggedForDelete;
    /// <summary>
    /// 获取建筑物是否被标记为删除状态
    /// </summary>
    public bool flaggedForDelete => _flaggedForDelete;

    /// <summary>
    /// 初始化建筑物组件，获取渲染器和默认材质
    /// </summary>
    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer) _defaultMaterial = _renderer.material;
    }

    /// <summary>
    /// 更新建筑物的渲染材质
    /// </summary>
    /// <param name="newMaterial">要应用的新材质</param>
    public void UpdateMaterial(Material newMaterial)
    {
        // 只有当新材质与当前材质不同时才进行更新
        if (_renderer.material != newMaterial) _renderer.material = newMaterial;
    }

    /// <summary>
    /// 放置建筑物时初始化渲染器和材质
    /// </summary>
    public void PlaceBuilding()
    {
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer) _defaultMaterial = _renderer.material;
        
        UpdateMaterial(_defaultMaterial);
    }

    /// <summary>
    /// 标记建筑物为待删除状态，并应用删除材质
    /// </summary>
    /// <param name="deleteMat">删除状态下的材质</param>
    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterial(deleteMat);
        _flaggedForDelete = true;
    }

    /// <summary>
    /// 移除建筑物的删除标记，恢复默认材质
    /// </summary>
    public void RemoveDeleteFlag()
    {
        UpdateMaterial(_defaultMaterial);
        _flaggedForDelete = false;
    }
}

