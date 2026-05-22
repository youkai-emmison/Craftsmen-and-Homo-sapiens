// Script purpose: Checks whether the player is touching the ground.
// Key Inspector variables:
// - groundCheckPoint: Transform placed near the player's feet.
// - checkRadius: Circle radius used for ground detection.
// - groundLayer: LayerMask that marks solid ground and platforms.
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    // Foot-level point where the overlap circle is centered.
    public Transform groundCheckPoint;

    // Size of the overlap circle used for ground detection.
    public float checkRadius = 0.15f;

    // Layers counted as ground.
    public LayerMask groundLayer;

    // True when the configured circle overlaps a ground collider.
    public bool IsGrounded { get; private set; }

    // Prevents the same missing-reference error from filling the Console.
    private bool hasLoggedMissingGroundCheckPoint;

    private void FixedUpdate()
    {
        // Ground checks run with physics so jump decisions use the latest collision state.
        UpdateGroundedState();
    }

    private void UpdateGroundedState()
    {
        if (groundCheckPoint == null)
        {
            if (!hasLoggedMissingGroundCheckPoint)
            {
                Debug.LogError("GroundChecker: Ground Check Point is not assigned.", this);
                hasLoggedMissingGroundCheckPoint = true;
            }

            IsGrounded = false;
            return;
        }

        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);
        IsGrounded = groundCollider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }

        // This shows the exact ground check radius configured in the Inspector.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
    }
}
