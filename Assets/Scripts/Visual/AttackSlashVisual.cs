// Script purpose: Shows a short-lived slash marker when the player attacks.
// Key Inspector variables:
// - slashRenderer: SpriteRenderer used for the slash flash.
// - showDuration: How long the slash stays visible.
using System.Collections;
using UnityEngine;

public class AttackSlashVisual : MonoBehaviour
{
    // Renderer enabled briefly when an attack happens.
    public SpriteRenderer slashRenderer;

    // Seconds before hiding the slash again.
    public float showDuration = 0.12f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        ValidateRequiredReferences();
        HideSlash();
    }

    public void Play(Vector3 worldPosition, float facingDirectionX)
    {
        if (slashRenderer == null)
        {
            return;
        }

        transform.position = worldPosition;
        transform.localScale = new Vector3(Mathf.Sign(facingDirectionX), 1f, 1f);
        slashRenderer.enabled = true;

        RestartHideCoroutine();
    }

    private void ValidateRequiredReferences()
    {
        if (slashRenderer == null)
        {
            Debug.LogError("AttackSlashVisual: Slash Renderer is not assigned.", this);
        }
    }

    private void RestartHideCoroutine()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        HideSlash();
    }

    private void HideSlash()
    {
        if (slashRenderer != null)
        {
            slashRenderer.enabled = false;
        }
    }
}
