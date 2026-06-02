using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局装置管理器 —— 限制场上装置数量。
/// 挂载在场景中的持久 GameObject 上（如 GameManager）。
/// </summary>
public class DeviceManager : MonoBehaviour
{
    #region 单例

    public static DeviceManager Instance { get; private set; }

    #endregion

    #region 常量

    /// <summary>场上最大装置总数。</summary>
    public const int MAX_TOTAL_DEVICES = 8;

    #endregion

    #region 运行时状态

    // 装置计数：key = deviceName, value = 当前数量
    private readonly Dictionary<string, int> deviceTypeCounts = new Dictionary<string, int>();

    // 当前场上装置总数
    private int currentTotalCount;

    #endregion

    #region 属性

    public int CurrentTotalCount => currentTotalCount;

    #endregion

    #region 生命周期

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion

    #region 公开方法

    /// <summary>
    /// 检查是否可以部署指定类型的装置。
    /// </summary>
    /// <param name="deviceName">装置名称（用于同种计数）</param>
    /// <param name="maxSameType">同种装置最大数量</param>
    /// <returns>是否可以部署</returns>
    public bool CanDeploy(string deviceName, int maxSameType)
    {
        // 检查总数限制
        if (currentTotalCount >= MAX_TOTAL_DEVICES)
            return false;

        // 检查同种数量限制
        int currentTypeCount = 0;
        if (deviceTypeCounts.TryGetValue(deviceName, out currentTypeCount))
        {
            if (currentTypeCount >= maxSameType)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 注册一个新部署的装置。
    /// </summary>
    /// <param name="deviceName">装置名称</param>
    public void RegisterDevice(string deviceName)
    {
        currentTotalCount++;

        if (deviceTypeCounts.ContainsKey(deviceName))
            deviceTypeCounts[deviceName]++;
        else
            deviceTypeCounts[deviceName] = 1;
    }

    /// <summary>
    /// 注销一个销毁的装置。
    /// </summary>
    /// <param name="deviceName">装置名称</param>
    public void UnregisterDevice(string deviceName)
    {
        currentTotalCount = Mathf.Max(0, currentTotalCount - 1);

        if (deviceTypeCounts.ContainsKey(deviceName))
        {
            deviceTypeCounts[deviceName] = Mathf.Max(0, deviceTypeCounts[deviceName] - 1);
            if (deviceTypeCounts[deviceName] == 0)
                deviceTypeCounts.Remove(deviceName);
        }
    }

    /// <summary>
    /// 获取指定类型的当前装置数量。
    /// </summary>
    public int GetTypeCount(string deviceName)
    {
        if (deviceTypeCounts.TryGetValue(deviceName, out int count))
            return count;
        return 0;
    }

    #endregion
}
