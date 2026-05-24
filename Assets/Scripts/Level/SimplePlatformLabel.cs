// Script purpose: Adds a readable label to greybox ground and platform objects.
// Key Inspector variables:
// - platformName: Human-readable platform name.
// - gameplayPurpose: Why this platform exists in the demo route.
using UnityEngine;

public class SimplePlatformLabel : MonoBehaviour
{
    // Clear name for teammates reading the scene hierarchy.
    public string platformName;

    // Short note explaining the platform's role in the route.
    public string gameplayPurpose;
}
