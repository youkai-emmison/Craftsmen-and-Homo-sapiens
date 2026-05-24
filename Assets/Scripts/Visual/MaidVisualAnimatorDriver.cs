// Script purpose: Drives the maid visual Animator without controlling gameplay movement.
// Key Inspector variables:
// - animator: Visual-only Animator on the MaidVisual child object.
// - velocitySource: Player Rigidbody2D used only to decide idle or walk.
using UnityEngine;

public class MaidVisualAnimatorDriver : MonoBehaviour
{
    // Visual-only Animator. Gameplay scripts stay on the Player root.
    public Animator animator;

    // Rigidbody2D used to decide whether the visual should idle or walk.
    public Rigidbody2D velocitySource;

    // Speed threshold before the visual enters the walk animation.
    public float movingThreshold = 0.05f;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HurtHash = Animator.StringToHash("Hurt");

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("MaidVisualAnimatorDriver: Animator is not assigned.", this);
        }

        if (velocitySource == null)
        {
            Debug.LogError("MaidVisualAnimatorDriver: Velocity Source is not assigned.", this);
        }
    }

    private void Update()
    {
        UpdateMovingState();
    }

    public void PlayAttack()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(AttackHash);
    }

    public void PlayHurt()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger(HurtHash);
    }

    private void UpdateMovingState()
    {
        if (animator == null || velocitySource == null)
        {
            return;
        }

        animator.SetBool(IsMovingHash, Mathf.Abs(velocitySource.velocity.x) > movingThreshold);
    }
}
