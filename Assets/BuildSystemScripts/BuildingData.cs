using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 建筑数据脚本对象，用于定义建筑的基本属性。
/// 可通过 Unity 编辑器菜单 "Building System/Build Data" 创建实例。
/// </summary>
[CreateAssetMenu(menuName = "Building System/Build Data")]
public class BuildingData : ScriptableObject
{
    /// <summary>
    /// 显示名称，用于在 UI 中展示该建筑的名称。
    /// </summary>
    public string displayName;

    /// <summary>
    /// 图标资源，表示该建筑在界面中的图标显示。
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// 网格吸附尺寸，控制建筑放置时与网格对齐的单位大小。
    /// </summary>
    public float gridSnapSize;

    /// <summary>
    /// 预制体引用，表示实际生成到场景中的建筑 GameObject。
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// 建筑尺寸，描述建筑在世界坐标系中所占的空间大小。
    /// </summary>
    public Vector3 buildingSize;

    /// <summary>
    /// 构件类型，标识该建筑属于房间、走廊还是装饰物等类别。
    /// </summary>
    public PartType partType;
}

/// <summary>
/// 构件类型的枚举，用来区分不同种类的建筑构件。
/// </summary>
public enum PartType
{
    /// <summary>
    /// 房间类型构件。
    /// </summary>
    Room = 0,

    /// <summary>
    /// 走廊类型构件。
    /// </summary>
    Corridor = 1,

    /// <summary>
    /// 装饰性构件。
    /// </summary>
    Decoration = 2
}

#if UNITY_EDITOR

/// <summary>
/// 自定义编辑器类，用于为 BuildingData 资源提供静态预览图支持。
/// 在 Project 视图或 Inspector 中查看资源时会调用 RenderStaticPreview 方法生成缩略图。
/// </summary>
[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
    /// <summary>
    /// 渲染资源的静态预览图像，在 Project 窗口或 Inspector 的预览区域使用。
    /// </summary>
    /// <param name="assetPath">当前资源文件路径。</param>
    /// <param name="subAssets">子资产数组（未使用）。</param>
    /// <param name="width">期望输出纹理宽度。</param>
    /// <param name="height">期望输出纹理高度。</param>
    /// <returns>生成的 Texture2D 类型预览图；若无法生成则返回 null。</returns>
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (BuildingData)target;
        if (data == null || data.icon == null) return null;
    
        var sprite = data.icon;
        var sourceTex = sprite.texture;
        if (sourceTex == null) return null;
    
        // 使用 RenderTexture 来裁剪并缩放 Sprite 图像区域以适配目标尺寸
        var rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
    
        // 设置像素矩阵，并绘制 Sprite 对应部分到 RenderTexture 上
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, width, height, 0);
        Graphics.DrawTexture(
            new Rect(0, 0, width, height),
            sourceTex,
            new Rect(
                sprite.rect.x / sourceTex.width,
                sprite.rect.y / sourceTex.height,
                sprite.rect.width / sourceTex.width,
                sprite.rect.height / sourceTex.height
            ),
            0, 0, 0, 0
        );
        GL.PopMatrix();
    
        // 将 RenderTexture 内容读取为 Texture2D 并应用更改
        var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
    
        // 恢复原始状态并释放临时 RenderTexture
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
    
        return tex;
    }
}

#endif