// Script purpose: Updates the player's visual facing direction and attack point side.
// Key Inspector variables:
// - visualRoot: Player visual Transform flipped left or right.
// - attackPoint: Melee hit center moved to the facing side.
// - attackEffectTransform: Optional slash visual moved and flipped with the attack point.
// - horizontalOffset: Local X distance used for attack point placement.
using UnityEngine;

public class PlayerFacingController : MonoBehaviour
{
    // Visual root flipped when the player changes facing direction.
    public Transform visualRoot;

    // Attack point moved to the left or right of the player.
    public Transform attackPoint;

    // Optional visible slash Transform kept on the same side as attackPoint.
    public Transform attackEffectTransform;

    // Local X distance from the player center to the attack point.
    public float horizontalOffset = 0.85f;

    // Current facing direction: 1 means right, -1 means left.
    private int facingDirection = 1;

    private void Awake()
    {
        ValidateRequiredReferences();
        ApplyFacing();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0.01f)
        {
            SetFacingDirection(1);
        }
        else if (horizontalInput < -0.01f)
        {
            SetFacingDirection(-1);
        }
    }

    private void ValidateRequiredReferences()
    {
        if (visualRoot == null)
        {
            Debug.LogError("PlayerFacingController: Visual Root is not assigned.", this);
        }

        if (attackPoint == null)
        {
            Debug.LogError("PlayerFacingController: Attack Point is not assigned.", this);
        }
    }

    private void SetFacingDirection(int nextFacingDirection)
    {
        if (facingDirection == nextFacingDirection)
        {
            return;
        }

        facingDirection = nextFacingDirection;
        ApplyFacing();
    }

    private void ApplyFacing()
    {
        ApplyVisualFacing();
        ApplyAttackPointFacing();
        ApplyAttackEffectFacing();
    }

    private void ApplyVisualFacing()
    {
        if (visualRoot == null)
        {
            return;
        }

        Vector3 visualScale = visualRoot.localScale;
        visualScale.x = Mathf.Abs(visualScale.x) * facingDirection;
        visualRoot.localScale = visualScale;
    }

    private void ApplyAttackPointFacing()
    {
        if (attackPoint == null)
        {
            return;
        }

        Vector3 attackPointPosition = attackPoint.localPosition;
        attackPointPosition.x = horizontalOffset * facingDirection;
        attackPoint.localPosition = attackPointPosition;
    }

    private void ApplyAttackEffectFacing()
    {
        if (attackEffectTransform == null)
        {
            return;
        }

        Vector3 effectPosition = attackEffectTransform.localPosition;
        effectPosition.x = horizontalOffset * facingDirection;
        attackEffectTransform.localPosition = effectPosition;

        Vector3 effectScale = attackEffectTransform.localScale;
        effectScale.x = Mathf.Abs(effectScale.x) * facingDirection;
        attackEffectTransform.localScale = effectScale;
    }
}
