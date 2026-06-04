using UnityEngine;

/// <summary>
/// 机械装置数据定义 —— ScriptableObject，描述一个装置的所有静态属性。
/// 通过 Unity Inspector 的 Create → Equipment → Device Data 菜单创建。
/// </summary>
[CreateAssetMenu(fileName = "NewDevice", menuName = "Equipment/Device Data")]
public class DeviceData : ScriptableObject
{
    [Header("基本信息")]
    public string deviceName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("装置属性")]
    public int maxDurability = 5;           // 最大耐久
    public float duration = 20f;            // 持续时间（秒）
    public float attackInterval = 1f;       // 攻击间隔
    public float attackRange = 5f;          // 攻击范围
    public float projectileSpeed = 8f;      // 子弹速度

    [Header("伤害")]
    [Range(0f, 1f)]
    public float baseDamagePercent = 0.3f;  // 基于玩家伤害的百分比

    [Header("限制")]
    public int maxSameType = 3;             // 同种装置最大数量

    [Header("预制体")]
    public GameObject devicePrefab;         // 装置预制体
    public GameObject projectilePrefab;     // 子弹预制体
}
