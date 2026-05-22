// Script purpose: Labels a greybox platform or ground block for level-design review.
// Key Inspector variables:
// - platformName: Human-readable platform label.
// - gameplayPurpose: Why this platform exists in the prototype.
using UnityEngine;

public class SimplePlatformLabel : MonoBehaviour
{
    // Human-readable platform or ground name.
    public string platformName;

    // Short note explaining the intended gameplay use.
    public string gameplayPurpose;
}
