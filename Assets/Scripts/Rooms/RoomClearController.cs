// Script purpose: Unlocks the room exit after all assigned enemies are defeated.
// Key Inspector variables:
// - enemiesInRoom: EnemyHealth references that must all be dead.
// - exitDoor: Door unlocked when the room is clear.
using UnityEngine;

public class RoomClearController : MonoBehaviour
{
    // Enemy list manually assigned for this room.
    public EnemyHealth[] enemiesInRoom;

    // Exit door unlocked after all enemies are defeated.
    public ExitDoorController exitDoor;

    // Prevents repeated logs if the array contains a missing enemy reference.
    private bool hasLoggedNullEnemy;

    private void Start()
    {
        ValidateRequiredReferences();
        SubscribeToEnemies();

        if (enemiesInRoom == null || enemiesInRoom.Length == 0)
        {
            Debug.Log("RoomClearController: No enemies assigned, unlocking exit door.", this);
            UnlockExitDoor();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromEnemies();
    }

    private void ValidateRequiredReferences()
    {
        if (exitDoor == null)
        {
            Debug.LogError("RoomClearController: Exit Door is not assigned.", this);
        }
    }

    private void SubscribeToEnemies()
    {
        if (enemiesInRoom == null)
        {
            return;
        }

        foreach (EnemyHealth enemyHealth in enemiesInRoom)
        {
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDefeated += HandleEnemyDefeated;
            }
        }
    }

    private void UnsubscribeFromEnemies()
    {
        if (enemiesInRoom == null)
        {
            return;
        }

        foreach (EnemyHealth enemyHealth in enemiesInRoom)
        {
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDefeated -= HandleEnemyDefeated;
            }
        }
    }

    private void HandleEnemyDefeated(EnemyHealth defeatedEnemy)
    {
        if (AreAllEnemiesDefeated())
        {
            UnlockExitDoor();
        }
    }

    private bool AreAllEnemiesDefeated()
    {
        foreach (EnemyHealth enemyHealth in enemiesInRoom)
        {
            if (enemyHealth == null)
            {
                LogNullEnemy();
                return false;
            }

            if (!enemyHealth.IsDead)
            {
                return false;
            }
        }

        return true;
    }

    private void UnlockExitDoor()
    {
        if (exitDoor != null)
        {
            exitDoor.UnlockDoor();
        }
    }

    private void LogNullEnemy()
    {
        if (hasLoggedNullEnemy)
        {
            return;
        }

        Debug.LogError("RoomClearController: Enemies In Room contains a missing EnemyHealth reference.", this);
        hasLoggedNullEnemy = true;
    }
}
