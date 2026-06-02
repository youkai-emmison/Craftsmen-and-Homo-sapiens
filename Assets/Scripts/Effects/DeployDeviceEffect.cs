using UnityEngine;

/// <summary>
/// 部署装置效果 —— 装备武器时在玩家位置部署装置。
/// 作为 ItemEffect 附加到 DeviceEquipmentData 的 effects 数组。
/// </summary>
[CreateAssetMenu(fileName = "DeployDevice", menuName = "Item Effects/Deploy Device")]
public class DeployDeviceEffect : ItemEffect
{
    [Header("部署配置")]
    public DeviceData deviceData;

    [Header("部署偏移")]
    public Vector2 deployOffset = new Vector2(1f, 0f); // 相对玩家的部署偏移

    private GameObject deployedDevice;

    public override void ExecuteEffect(Transform target)
    {
        if (deviceData == null || deviceData.devicePrefab == null)
        {
            Debug.LogWarning("DeployDeviceEffect: deviceData 或 devicePrefab 为空");
            return;
        }

        // 检查是否可以部署
        if (DeviceManager.Instance != null)
        {
            if (!DeviceManager.Instance.CanDeploy(deviceData.deviceName, deviceData.maxSameType))
            {
                Debug.Log($"无法部署 {deviceData.deviceName}：达到数量上限");
                return;
            }
        }

        // 获取玩家属性
        PlayerStats playerStats = target.GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("DeployDeviceEffect: 找不到 PlayerStats");
            return;
        }

        // 计算部署位置（玩家前方）
        float facingDir = target.localScale.x > 0 ? 1f : -1f;
        Vector2 deployPos = (Vector2)target.position + new Vector2(deployOffset.x * facingDir, deployOffset.y);

        // 实例化装置
        deployedDevice = Instantiate(deviceData.devicePrefab, deployPos, Quaternion.identity);

        // 初始化装置控制器
        DeviceController controller = deployedDevice.GetComponent<DeviceController>();
        if (controller != null)
        {
            controller.Initialize(deviceData, playerStats);
        }
    }

    public override void CancelEffect(Transform target)
    {
        // 装备卸下时，可以选择销毁已部署的装置或保留
        // 这里选择保留，装置会自然到期销毁
    }
}
