// Script purpose: Updates a small world-space bar that displays PlayerSkillEnergy.
// Key Inspector variables:
// - fillTransform: Blue fill object scaled from 0 to 1 on X.
// - skillEnergy: Player skill-energy source; assign the Player component in the Inspector.
using UnityEngine;

public class SimpleSkillEnergyBar : MonoBehaviour
{
    // Fill object scaled horizontally by the current skill-energy ratio.
    public Transform fillTransform;

    // Player skill-energy source shown by this bar.
    public PlayerSkillEnergy skillEnergy;

    private void Awake()
    {
        if (fillTransform == null)
        {
            Debug.LogError("SimpleSkillEnergyBar: Fill Transform is not assigned.", this);
        }

        if (skillEnergy == null)
        {
            Debug.LogError("SimpleSkillEnergyBar: PlayerSkillEnergy is not assigned.", this);
        }
    }

    private void Update()
    {
        if (fillTransform == null || skillEnergy == null)
        {
            return;
        }

        float ratio = skillEnergy.MaxSkillEnergy > 0
            ? Mathf.Clamp01((float)skillEnergy.CurrentSkillEnergy / skillEnergy.MaxSkillEnergy)
            : 0f;
        fillTransform.localScale = new Vector3(ratio, 1f, 1f);
    }
}
