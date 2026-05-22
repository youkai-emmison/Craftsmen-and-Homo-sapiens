// Script purpose: Labels a greybox boundary so collaborators understand its role.
// Key Inspector variables:
// - boundaryName: Human-readable boundary label.
// - blocksPlayer: Whether this boundary is intended to physically block the player.
using UnityEngine;

public class RoomBoundary : MonoBehaviour
{
    // Human-readable label shown in the Inspector.
    public string boundaryName;

    // Documentation flag; the Collider2D actually performs the blocking.
    public bool blocksPlayer = true;
}
