// Script purpose: Shows a short attack slash when the player performs an accepted attack.
// Key Inspector variables:
// - attackSpriteRenderer: SpriteRenderer used for the slash.
// - visibleDuration: How long the slash stays visible.
using System.Collections;
using UnityEngine;

public class PlayerAttackVisual : MonoBehaviour
{
    // Slash renderer enabled briefly when an attack starts.
    public SpriteRenderer attackSpriteRenderer;

    // Seconds the slash remains visible.
    public float visibleDuration = 0.12f;

    // Currently running hide routine, if any.
    private Coroutine activeVisualRoutine;

    private void Awake()
    {
        if (attackSpriteRenderer == null)
        {
            Debug.LogError("PlayerAttackVisual: Attack Sprite Renderer is not assigned.", this);
            return;
        }

        attackSpriteRenderer.enabled = false;
    }

    public void PlayAttack()
    {
        if (attackSpriteRenderer == null)
        {
            return;
        }

        if (activeVisualRoutine != null)
        {
            StopCoroutine(activeVisualRoutine);
        }

        activeVisualRoutine = StartCoroutine(ShowAttackRoutine());
    }

    private IEnumerator ShowAttackRoutine()
    {
        attackSpriteRenderer.enabled = true;

        yield return new WaitForSeconds(visibleDuration);

        attackSpriteRenderer.enabled = false;
        activeVisualRoutine = null;
    }
}
