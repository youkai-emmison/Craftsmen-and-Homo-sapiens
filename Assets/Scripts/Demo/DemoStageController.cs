// Script purpose: Coordinates the Early, Mid, and Boss demo room clear sequence.
// Key Inspector variables:
// - currentStage: Current compressed demo stage.
// - earlyRoom / midRoom / bossRoom: Room clear controllers assigned manually.
// - earlyExitDoor / midExitDoor / bossExitDoor: Exit doors for each room.
using UnityEngine;

public class DemoStageController : MonoBehaviour
{
    // Current stage visible in the Inspector while testing.
    public DemoStageType currentStage = DemoStageType.Early;

    // First room clear controller.
    public RoomClearController earlyRoom;

    // Second room clear controller.
    public RoomClearController midRoom;

    // Boss room clear controller.
    public RoomClearController bossRoom;

    // Door that opens after Early Room is cleared.
    public ExitDoorController earlyExitDoor;

    // Door that opens after Mid Room is cleared.
    public ExitDoorController midExitDoor;

    // Door that opens after Boss Room is cleared.
    public ExitDoorController bossExitDoor;

    private void Awake()
    {
        ValidateRequiredReferences();
    }

    private void OnEnable()
    {
        SubscribeToRooms();
    }

    private void Start()
    {
        Debug.Log("DemoStageController: Early Room started.", this);
    }

    private void OnDisable()
    {
        UnsubscribeFromRooms();
    }

    private void ValidateRequiredReferences()
    {
        if (earlyRoom == null || midRoom == null || bossRoom == null)
        {
            Debug.LogError("DemoStageController: Early, Mid, and Boss Room references must be assigned.", this);
        }

        if (earlyExitDoor == null || midExitDoor == null || bossExitDoor == null)
        {
            Debug.LogError("DemoStageController: Early, Mid, and Boss Exit Door references must be assigned.", this);
        }
    }

    private void SubscribeToRooms()
    {
        if (earlyRoom != null)
        {
            earlyRoom.OnRoomCleared += HandleEarlyRoomCleared;
        }

        if (midRoom != null)
        {
            midRoom.OnRoomCleared += HandleMidRoomCleared;
        }

        if (bossRoom != null)
        {
            bossRoom.OnRoomCleared += HandleBossRoomCleared;
        }
    }

    private void UnsubscribeFromRooms()
    {
        if (earlyRoom != null)
        {
            earlyRoom.OnRoomCleared -= HandleEarlyRoomCleared;
        }

        if (midRoom != null)
        {
            midRoom.OnRoomCleared -= HandleMidRoomCleared;
        }

        if (bossRoom != null)
        {
            bossRoom.OnRoomCleared -= HandleBossRoomCleared;
        }
    }

    private void HandleEarlyRoomCleared(RoomClearController clearedRoom)
    {
        currentStage = DemoStageType.Mid;
        UnlockDoor(earlyExitDoor);
        Debug.Log("DemoStageController: Early Room cleared. Mid Room unlocked.", this);
    }

    private void HandleMidRoomCleared(RoomClearController clearedRoom)
    {
        currentStage = DemoStageType.Boss;
        UnlockDoor(midExitDoor);
        Debug.Log("DemoStageController: Mid Room cleared. Boss Room unlocked.", this);
    }

    private void HandleBossRoomCleared(RoomClearController clearedRoom)
    {
        UnlockDoor(bossExitDoor);
        Debug.Log("Demo Complete / Boss Defeated", this);
    }

    private void UnlockDoor(ExitDoorController exitDoor)
    {
        if (exitDoor != null)
        {
            exitDoor.UnlockDoor();
        }
    }
}
