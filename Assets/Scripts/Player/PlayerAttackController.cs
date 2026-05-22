// Script purpose: Reads player attack input and triggers one melee hit check per cooldown.
// Key Inspector variables:
// - attackCooldown: Minimum time between attack attempts.
// - meleeHitDetector: Component that performs the actual melee overlap check.
// - attackVisual: Optional component that shows a short attack slash.
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // Minimum time in seconds before another attack can start.
    public float attackCooldown = 0.35f;

    // Required detector called once when an attack is accepted.
    public MeleeHitDetector meleeHitDetector;

    // Optional visual feedback played when an attack is accepted.
    public PlayerAttackVisual attackVisual;

    // Timestamp gate that prevents click-spam attacks.
    private float nextAttackAllowedTime;

    private void Awake()
    {
        if (meleeHitDetector == null)
        {
            Debug.LogError("PlayerAttackController: MeleeHitDetector is not assigned. Drag the player's MeleeHitDetector into this field.", this);
        }
    }

    private void Update()
    {
        // Only key-down events request attacks; holding the button does not repeat every frame.
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

        // The cooldown gate prevents rapid clicking from triggering repeated hit checks.
        nextAttackAllowedTime = Time.time + attackCooldown;
        PlayAttackVisual();
        meleeHitDetector.DetectHit();
    }

    private void PlayAttackVisual()
    {
        if (attackVisual == null)
        {
            return;
        }

        attackVisual.PlayAttack();
    }
}
