// Script purpose: Grants demo experience to PlayerDemoStats when the player touches this pickup.
// Key Inspector variables:
// - experienceAmount: Experience granted by this pickup.
using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    // Experience granted when Player collects this pickup.
    public int experienceAmount = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerDemoStats playerDemoStats = other.GetComponent<PlayerDemoStats>();

        if (playerDemoStats == null)
        {
            Debug.LogError("ExperiencePickup: PlayerDemoStats is not assigned on the Player object.", this);
            return;
        }

        playerDemoStats.AddExperience(experienceAmount);
        Debug.Log($"ExperiencePickup: Player collected {experienceAmount} experience.", this);
        Destroy(gameObject);
    }
}
