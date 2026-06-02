// Script purpose: Connects cleared rooms and exit doors into the three-stage demo flow.
// Key Inspector variables:
// - currentStage: Current demo stage shown for debugging.
// - earlyRoom / midRoom / bossRoom: Room clear controllers assigned by hand.
// - earlyExitDoor / midExitDoor / bossExitDoor: Doors that move the demo forward.
using UnityEngine;

public class DemoStageController : MonoBehaviour
{
    // Current stage is visible in the Inspector while testing the demo route.
    public DemoStageType currentStage = DemoStageType.Early;

    // Room controllers notify this script when enemies in each room are cleared.
    public RoomClearController earlyRoom;
    public RoomClearController midRoom;
    public RoomClearController bossRoom;

    // Doors notify this script when the player enters an unlocked exit.
    public ExitDoorController earlyExitDoor;
    public ExitDoorController midExitDoor;
    public ExitDoorController bossExitDoor;

    private bool isDemoComplete;

    private void OnEnable()
    {
        SubscribeToRooms();
        SubscribeToDoors();
    }

    private void Start()
    {
        Debug.Log("Demo started: Early Room.", this);
    }

    private void OnDisable()
    {
        UnsubscribeFromRooms();
        UnsubscribeFromDoors();
    }

    private void SubscribeToRooms()
    {
        if (earlyRoom != null) earlyRoom.OnRoomCleared += HandleRoomCleared;
        if (midRoom != null) midRoom.OnRoomCleared += HandleRoomCleared;
        if (bossRoom != null) bossRoom.OnRoomCleared += HandleRoomCleared;
    }

    private void UnsubscribeFromRooms()
    {
        if (earlyRoom != null) earlyRoom.OnRoomCleared -= HandleRoomCleared;
        if (midRoom != null) midRoom.OnRoomCleared -= HandleRoomCleared;
        if (bossRoom != null) bossRoom.OnRoomCleared -= HandleRoomCleared;
    }

    private void SubscribeToDoors()
    {
        if (earlyExitDoor != null) earlyExitDoor.OnUnlockedDoorEntered += HandleDoorEntered;
        if (midExitDoor != null) midExitDoor.OnUnlockedDoorEntered += HandleDoorEntered;
        if (bossExitDoor != null) bossExitDoor.OnUnlockedDoorEntered += HandleDoorEntered;
    }

    private void UnsubscribeFromDoors()
    {
        if (earlyExitDoor != null) earlyExitDoor.OnUnlockedDoorEntered -= HandleDoorEntered;
        if (midExitDoor != null) midExitDoor.OnUnlockedDoorEntered -= HandleDoorEntered;
        if (bossExitDoor != null) bossExitDoor.OnUnlockedDoorEntered -= HandleDoorEntered;
    }

    private void HandleRoomCleared(RoomClearController clearedRoom)
    {
        if (clearedRoom == earlyRoom)
        {
            Debug.Log("Early Room cleared. Enter the door to reach Mid Room.", this);
            return;
        }

        if (clearedRoom == midRoom)
        {
            Debug.Log("Mid Room cleared. Enter the door to reach Boss Room.", this);
            return;
        }

        if (clearedRoom == bossRoom)
        {
            Debug.Log("Boss defeated. Enter the final door to finish the demo.", this);
        }
    }

    private void HandleDoorEntered(ExitDoorController enteredDoor)
    {
        if (enteredDoor == earlyExitDoor)
        {
            MoveToStage(DemoStageType.Mid, "Entered Mid Room.");
            return;
        }

        if (enteredDoor == midExitDoor)
        {
            MoveToStage(DemoStageType.Boss, "Entered Boss Room.");
            return;
        }

        if (enteredDoor == bossExitDoor)
        {
            CompleteDemo();
        }
    }

    private void MoveToStage(DemoStageType nextStage, string message)
    {
        if (isDemoComplete)
        {
            return;
        }

        currentStage = nextStage;
        Debug.Log(message, this);
    }

    private void CompleteDemo()
    {
        if (isDemoComplete)
        {
            return;
        }

        isDemoComplete = true;
        Debug.Log("Demo Complete / Boss Defeated", this);
    }
}
