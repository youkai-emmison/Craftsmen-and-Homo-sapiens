// Script purpose: Keeps a world-space marker facing the camera.
// Key Inspector variables:
// - cameraTransform: Camera Transform whose rotation should be copied.
using UnityEngine;

public class RuntimeBillboard : MonoBehaviour
{
    // Camera transform assigned manually in the Inspector.
    public Transform cameraTransform;

    // Prevents repeated missing-reference logs.
    private bool hasLoggedMissingCameraTransform;

    private void LateUpdate()
    {
        if (cameraTransform == null)
        {
            LogMissingCameraTransform();
            return;
        }

        // Copy camera rotation so simple world-space markers stay readable.
        transform.rotation = cameraTransform.rotation;
    }

    private void LogMissingCameraTransform()
    {
        if (hasLoggedMissingCameraTransform)
        {
            return;
        }

        Debug.LogError("RuntimeBillboard: Camera Transform is not assigned.", this);
        hasLoggedMissingCameraTransform = true;
    }
}
