// Script purpose: Reads player attack input and triggers one melee hit check per cooldown.
// Key Inspector variables:
// - attackCooldown: Minimum time between attack attempts.
// - meleeHitDetector: Component that performs the actual melee overlap check.
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // Minimum time in seconds before another attack can start.
    public float attackCooldown = 0.35f;

    // Required detector called once when an attack is accepted.
    public MeleeHitDetector meleeHitDetector;

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
    }
}
