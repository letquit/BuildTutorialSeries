using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 世界网格工具类，提供网格相关的计算功能
/// </summary>
public static class WorldGrid
{
    /// <summary>
    /// 将世界坐标点转换为网格坐标点
    /// </summary>
    /// <param name="worldPos">世界坐标位置</param>
    /// <param name="gridScale">网格缩放比例，即每个网格单元的大小</param>
    /// <returns>对应的世界坐标点在网格中的位置</returns>
    public static Vector3 GridPositionFromWorldPoint3D(Vector3 worldPos, float gridScale)
    {
        // 将世界坐标按网格比例缩放后四舍五入，再乘以网格比例得到网格坐标
        var x = Mathf.Round(worldPos.x / gridScale) * gridScale;
        var y = Mathf.Round(worldPos.y / gridScale) * gridScale;
        var z = Mathf.Round(worldPos.z / gridScale) * gridScale;
        
        return new Vector3(x, y, z);
    }
}
