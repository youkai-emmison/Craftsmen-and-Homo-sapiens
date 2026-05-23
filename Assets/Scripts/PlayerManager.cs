using UnityEngine;

/// <summary>
/// 玩家管理器 —— 全局单例。
/// 持有玩家引用，提供全局访问玩家的方式。
/// 其他系统（如敌人 AI、UI、相机）通过此类获取玩家信息。
/// </summary>
public class PlayerManager : MonoBehaviour
{
    #region 单例

    public static PlayerManager Instance { get; private set; }

    #endregion

    #region 引用

    [Header("玩家引用")]
    [SerializeField] private Player player; // 场景中的 Player 组件

    #endregion

    #region 生命周期

    private void Awake()
    {
        // 单例初始化：确保全局只有一个 PlayerManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion

    #region 公开接口

    /// <summary>
    /// 获取玩家 Player 组件。可能为 null（玩家未生成或已死亡）。
    /// </summary>
    public Player GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// 检查玩家是否存活（存在且生命值 > 0）。
    /// </summary>
    public bool IsPlayerAlive()
    {
        return player != null && player.currentHealth > 0f;
    }

    #endregion
}
