// Script purpose: Smoothly follows a target Transform from LateUpdate.
// Key Inspector variables:
// - target: Transform the camera should follow.
// - offset: Camera offset from the target, with Z usually kept at -10.
// - smoothTime: SmoothDamp time used for follow easing.
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Target transform, usually the Player.
    public Transform target;

    // Camera offset from the target; keep Z negative for 2D rendering.
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Lower values make the camera follow more tightly.
    public float smoothTime = 0.15f;

    // Internal SmoothDamp velocity.
    private Vector3 followVelocity = Vector3.zero;

    // Prevents repeated missing-target warnings.
    private bool hasWarnedMissingTarget;

    private void LateUpdate()
    {
        // LateUpdate follows the player's final position after movement has finished for the frame.
        if (target == null)
        {
            if (!hasWarnedMissingTarget)
            {
                Debug.LogWarning("CameraFollow: Target is not assigned. Drag the Player Transform into the Target field.", this);
                hasWarnedMissingTarget = true;
            }

            return;
        }

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref followVelocity, smoothTime);
    }
}
