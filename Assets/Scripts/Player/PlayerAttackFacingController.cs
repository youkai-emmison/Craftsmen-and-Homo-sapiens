// Script purpose: Keeps the melee attack point on the player's last facing side.
// Key Inspector variables:
// - attackPoint: Child transform moved left or right of the player.
// - visualRoot: Optional visual-only child flipped to match facing.
// - horizontalOffset / verticalOffset: Local attack point placement.
using UnityEngine;

public class PlayerAttackFacingController : MonoBehaviour
{
    // Attack point child used by MeleeHitDetector.
    public Transform attackPoint;

    // Optional visual root. Gameplay collider stays on the player root.
    public Transform visualRoot;

    // Horizontal local distance from player center.
    public float horizontalOffset = 0.85f;

    // Vertical local distance from player center.
    public float verticalOffset = 0f;

    // Last non-zero horizontal direction.
    private float facingDirectionX = 1f;

    public float FacingDirectionX => facingDirectionX;

    private void Awake()
    {
        if (attackPoint == null)
        {
            Debug.LogError("PlayerAttackFacingController: Attack Point is not assigned.", this);
        }
    }

    private void Update()
    {
        ReadFacingInput();
        MoveAttackPoint();
        FlipVisualRoot();
    }

    private void ReadFacingInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0f)
        {
            facingDirectionX = 1f;
        }
        else if (horizontalInput < 0f)
        {
            facingDirectionX = -1f;
        }
    }

    private void MoveAttackPoint()
    {
        if (attackPoint == null)
        {
            return;
        }

        attackPoint.localPosition = new Vector3(horizontalOffset * facingDirectionX, verticalOffset, 0f);
    }

    private void FlipVisualRoot()
    {
        if (visualRoot == null)
        {
            return;
        }

        Vector3 localScale = visualRoot.localScale;
        localScale.x = Mathf.Abs(localScale.x) * facingDirectionX;
        visualRoot.localScale = localScale;
    }
}
