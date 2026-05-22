// Script purpose: Handles locked/unlocked door feedback and room-clear trigger logs.
// Key Inspector variables:
// - IsUnlocked: Whether the door currently allows room exit.
// - doorSpriteRenderer: Optional renderer tinted by locked/unlocked color.
// - lockedColor / unlockedColor: Door colors for prototype feedback.
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    // Current lock state shown in the Inspector for quick testing.
    public bool IsUnlocked;

    // Optional renderer used for color feedback.
    public SpriteRenderer doorSpriteRenderer;

    // Color shown while the door is locked.
    public Color lockedColor = new Color(0.35f, 0.22f, 0.42f, 1f);

    // Color shown after the room is cleared.
    public Color unlockedColor = new Color(0.95f, 0.52f, 0.78f, 1f);

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

        if (IsUnlocked)
        {
            Debug.Log("Room Cleared / Next Room Unlocked", this);
            return;
        }

        Debug.Log("Door is locked.", this);
    }

    private void ApplyDoorColor()
    {
        if (doorSpriteRenderer == null)
        {
            return;
        }

        doorSpriteRenderer.color = IsUnlocked ? unlockedColor : lockedColor;
    }
}
