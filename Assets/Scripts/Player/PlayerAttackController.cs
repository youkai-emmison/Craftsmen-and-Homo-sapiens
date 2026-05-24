// Script purpose: Reads player attack input and triggers one melee hit check per cooldown.
// Key Inspector variables:
// - attackCooldown: Minimum time between attack attempts.
// - meleeHitDetector: Component that performs the actual melee overlap check.
// - attackSlashVisual: Optional visual flash shown when the attack is accepted.
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // Minimum time in seconds before another attack can start.
    public float attackCooldown = 0.35f;

    // Required detector called once when an attack is accepted.
    public MeleeHitDetector meleeHitDetector;

    // Optional slash marker used only for demo readability.
    public AttackSlashVisual attackSlashVisual;

    // Optional facing controller used to flip the slash direction.
    public PlayerAttackFacingController attackFacingController;

    // Timestamp gate that prevents click-spam attacks.
    private float nextAttackAllowedTime;

    private void Awake()
    {
        if (meleeHitDetector == null)
        {
            Debug.LogError("PlayerAttackController: MeleeHitDetector is not assigned.", this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (meleeHitDetector == null)
        {
            return;
        }

        if (Time.time < nextAttackAllowedTime)
        {
            return;
        }

        nextAttackAllowedTime = Time.time + attackCooldown;
        meleeHitDetector.DetectHit();
        PlayAttackVisual();
    }

    private void PlayAttackVisual()
    {
        if (attackSlashVisual == null || meleeHitDetector == null || meleeHitDetector.attackPoint == null)
        {
            return;
        }

        float facingDirectionX = attackFacingController != null ? attackFacingController.FacingDirectionX : Mathf.Sign(meleeHitDetector.attackPoint.localPosition.x);
        attackSlashVisual.Play(meleeHitDetector.attackPoint.position, facingDirectionX);
    }
}
