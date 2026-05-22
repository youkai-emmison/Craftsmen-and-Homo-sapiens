// Script purpose: Plays a short color flash on an assigned SpriteRenderer.
// Key Inspector variables:
// - targetRenderer: Renderer that flashes when PlayFlash is called.
// - flashColor: Temporary color used during the flash.
// - flashDuration: Duration of the flash in seconds.
using System.Collections;
using UnityEngine;

public class SimpleHitFlash : MonoBehaviour
{
    // Renderer that receives the temporary flash color.
    public SpriteRenderer targetRenderer;

    // Color applied during the flash.
    public Color flashColor = Color.white;

    // Flash duration in seconds.
    public float flashDuration = 0.08f;

    // Currently running flash coroutine, if any.
    private Coroutine activeFlashRoutine;

    // Prevents repeated missing-reference logs.
    private bool hasLoggedMissingTargetRenderer;

    public void PlayFlash()
    {
        if (targetRenderer == null)
        {
            LogMissingTargetRenderer();
            return;
        }

        if (activeFlashRoutine != null)
        {
            StopCoroutine(activeFlashRoutine);
        }

        activeFlashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Color originalColor = targetRenderer.color;
        targetRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        if (targetRenderer != null)
        {
            targetRenderer.color = originalColor;
        }

        activeFlashRoutine = null;
    }

    private void LogMissingTargetRenderer()
    {
        if (hasLoggedMissingTargetRenderer)
        {
            return;
        }

        Debug.LogError("SimpleHitFlash: Target Renderer is not assigned.", this);
        hasLoggedMissingTargetRenderer = true;
    }
}
