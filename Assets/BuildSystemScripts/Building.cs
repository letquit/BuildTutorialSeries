using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建筑物类，用于管理建筑物的渲染材质和删除标记状态。
/// 该类依赖 BoxCollider 组件，并在初始化时自动添加。
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    private BuildingData _assignedData;
    private BoxCollider _boxCollider;
    private GameObject _graphic;
    private Transform _colliders;
    private bool _isOverlapping;
    
    /// <summary>
    /// 获取分配给此建筑物的数据对象。
    /// </summary>
    public BuildingData assignedData => _assignedData;

    /// <summary>
    /// 获取一个布尔值，表示当前建筑物是否与其他物体发生重叠。
    /// </summary>
    public bool isOverlapping => _isOverlapping;
    
    private Renderer _renderer;
    private Material _defaultMaterial;

    private bool _flaggedForDelete;

    /// <summary>
    /// 获取建筑物是否被标记为删除状态。
    /// </summary>
    public bool flaggedForDelete => _flaggedForDelete;

    /// <summary>
    /// 初始化建筑物组件及其相关数据。
    /// 设置碰撞体大小、位置及触发器属性，并实例化预制件作为图形显示。
    /// 同时获取渲染器和默认材质。
    /// </summary>
    /// <param name="data">要绑定到此建筑物的 BuildingData 数据。</param>
    public void Init(BuildingData data)
    {
        _assignedData = data;
        
        // 配置碰撞体属性
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.size = _assignedData.buildingSize;
        _boxCollider.center = new Vector3(0, (_assignedData.buildingSize.y + .2f) * .5f, 0);
        _boxCollider.isTrigger = true;
        
        // 添加并配置刚体组件
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        // 实例化图形预制件并获取渲染相关组件
        _graphic = Instantiate(data.prefab, transform);
        _renderer = _graphic.GetComponentInChildren<Renderer>();
        _defaultMaterial = _renderer.material;

        // 查找并禁用碰撞器对象
        _colliders = _graphic.transform.Find("Colliders");
        if (_colliders != null) _colliders.gameObject.SetActive(false);
    }

    /// <summary>
    /// 在放置建筑物时调用。禁用主碰撞体并激活子级碰撞体（如果存在），
    /// 并将渲染材质设置回默认材质。
    /// </summary>
    public void PlaceBuilding()
    {
        _boxCollider.enabled = false;
        if (_colliders != null) _colliders.gameObject.SetActive(true);
        UpdateMaterial(_defaultMaterial);
        gameObject.layer = 10;
        gameObject.name = _assignedData.displayName + " - " + transform.position;
    }

    /// <summary>
    /// 更新建筑物模型使用的渲染材质。
    /// 只有当新的材质与当前材质不一致时才会执行更新操作。
    /// </summary>
    /// <param name="newMaterial">需要应用的新材质。</param>
    public void UpdateMaterial(Material newMaterial)
    {
        // 检查渲染器是否存在
        if (_renderer == null) return;
        // 只有当新材质与当前材质不同时才进行更新
        if (_renderer.material != newMaterial) _renderer.material = newMaterial;
    }

    /// <summary>
    /// 将建筑物标记为待删除状态，并切换至指定的删除状态材质。
    /// </summary>
    /// <param name="deleteMat">代表删除状态的材质。</param>
    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterial(deleteMat);
        _flaggedForDelete = true;
    }

    /// <summary>
    /// 清除建筑物的删除标记，并将其材质恢复为默认材质。
    /// </summary>
    public void RemoveDeleteFlag()
    {
        UpdateMaterial(_defaultMaterial);
        _flaggedForDelete = false;
    }

    /// <summary>
    /// 当其他 Collider 进入或持续留在本对象的触发区域内时调用。
    /// 标记当前建筑物处于重叠状态。
    /// </summary>
    /// <param name="other">进入触发区域的另一个 Collider 对象。</param>
    private void OnTriggerStay(Collider other)
    {
        _isOverlapping = true;
    }
    
    /// <summary>
    /// 当其他 Collider 离开本对象的触发区域时调用。
    /// 解除当前建筑物的重叠状态。
    /// </summary>
    /// <param name="other">离开触发区域的另一个 Collider 对象。</param>
    private void OnTriggerExit(Collider other)
    {
        _isOverlapping = false;
    }
}