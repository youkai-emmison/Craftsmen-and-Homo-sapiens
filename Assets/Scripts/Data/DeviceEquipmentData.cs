using UnityEngine;

/// <summary>
/// 装置装备数据 —— 将装置作为可装备的武器。
/// 继承自 EquipmentData，自动设置装备类型为 Device。
/// </summary>
[CreateAssetMenu(fileName = "New Device Equipment", menuName = "Equipment/Device Equipment")]
public class DeviceEquipmentData : EquipmentData
{
    [Header("装置配置")]
    public DeviceData deviceData;

    private void OnValidate()
    {
        equipmentType = EquipmentType.Device;
        itemType = ItemType.Equipment;
    }
}
