// Script purpose: Handles locked/unlocked door feedback and notifies demo flow when the player enters.
// Key Inspector variables:
// - IsUnlocked: Whether the door currently allows room exit.
// - doorSpriteRenderer: Optional renderer tinted by locked/unlocked color.
// - lockedColor / unlockedColor: Door colors for prototype feedback.
using System;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    // Current lock state shown in the Inspector for quick testing.
    public bool IsUnlocked;

    // Optional renderer used for color feedback.
    public SpriteRenderer doorSpriteRenderer;

    // Color shown while the door is locked.
    public Color lockedColor = new Color(0.2f, 0.04f, 0.06f, 1f);

    // Color shown after the room is cleared.
    public Color unlockedColor = new Color(0.75f, 0.12f, 0.16f, 1f);

    // DemoStageController listens for this instead of the door controlling stage logic itself.
    public event Action<ExitDoorController> OnUnlockedDoorEntered;

    // Prevents a player standing inside the trigger from firing the same exit repeatedly.
    private bool hasBeenEntered;

    private void Awake()
    {
        ApplyDoorColor();
    }

    public void UnlockDoor()
    {
        if (IsUnlocked)
        {
            return;
        }

        IsUnlocked = true;
        ApplyDoorColor();
        Debug.Log("ExitDoor unlocked.", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!IsUnlocked)
        {
            Debug.Log("Door is locked.", this);
            return;
        }

        EnterDoor();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsUnlocked || !other.CompareTag("Player"))
        {
            return;
        }

        EnterDoor();
    }

    private void EnterDoor()
    {
        if (hasBeenEntered)
        {
            return;
        }

        hasBeenEntered = true;
        Debug.Log("Room Cleared / Next Room Unlocked", this);
        OnUnlockedDoorEntered?.Invoke(this);
    }

    private void ApplyDoorColor()
    {
        if (doorSpriteRenderer != null)
        {
            doorSpriteRenderer.color = IsUnlocked ? unlockedColor : lockedColor;
        }
    }
}
