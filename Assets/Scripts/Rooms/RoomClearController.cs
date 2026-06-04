// Script purpose: Unlocks the room exit after all assigned enemies are defeated.
// Key Inspector variables:
// - enemiesInRoom: Enemy references that must all be dead.
// - exitDoor: Door unlocked when the room is clear.
using System;
using UnityEngine;

public class RoomClearController : MonoBehaviour
{
    // Enemy list manually assigned for this room.
    public Enemy[] enemiesInRoom;

    // Exit door unlocked after all enemies are defeated.
    public ExitDoorController exitDoor;

    // True after this room has completed its clear check.
    public bool IsRoomCleared { get; private set; }

    // DemoStageController subscribes to this for room flow.
    public Action<RoomClearController> OnRoomCleared;

    private void Start()
    {
        if (exitDoor == null)
            Debug.LogError("RoomClearController: Exit Door is not assigned.", this);

        if (!HasAnyValidEnemy())
        {
            Debug.LogWarning("RoomClearController: enemiesInRoom 为空或全为 null，自动解锁出口。请在 Inspector 中拖入敌人引用。", this);
            UnlockExitDoor();
            return;
        }

        SubscribeToEnemies();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEnemies();
    }

    private void SubscribeToEnemies()
    {
        if (enemiesInRoom == null) return;

        foreach (Enemy enemy in enemiesInRoom)
        {
            if (enemy != null)
                enemy.OnDeath += HandleEnemyDefeated;
        }
    }

    /// <summary>
    /// 检查 enemiesInRoom 是否包含至少一个有效（非 null）的敌人引用。
    /// </summary>
    private bool HasAnyValidEnemy()
    {
        if (enemiesInRoom == null) return false;

        foreach (Enemy enemy in enemiesInRoom)
        {
            if (enemy != null) return true;
        }
        return false;
    }

    private void UnsubscribeFromEnemies()
    {
        if (enemiesInRoom == null) return;

        foreach (Enemy enemy in enemiesInRoom)
        {
            if (enemy != null)
                enemy.OnDeath -= HandleEnemyDefeated;
        }
    }

    private void HandleEnemyDefeated()
    {
        if (AreAllEnemiesDefeated())
            UnlockExitDoor();
    }

    private bool AreAllEnemiesDefeated()
    {
        foreach (Enemy enemy in enemiesInRoom)
        {
            if (enemy == null) continue;
            if (enemy.currentHealth > 0f) return false;
        }
        return true;
    }

    private void UnlockExitDoor()
    {
        if (IsRoomCleared) return;

        IsRoomCleared = true;

        if (exitDoor != null)
            exitDoor.UnlockDoor();

        OnRoomCleared?.Invoke(this);
    }
}
