// Script purpose: Labels a room boundary object for greybox readability.
// Key Inspector variables:
// - boundaryName: Human-readable boundary label.
// - blocksPlayer: Whether the attached Collider2D should block the player.
using UnityEngine;

public class RoomBoundary : MonoBehaviour
{
    // Clear name for this wall or ceiling in the Inspector.
    public string boundaryName;

    // Inspector note only; the Collider2D actually blocks movement.
    public bool blocksPlayer = true;
}
