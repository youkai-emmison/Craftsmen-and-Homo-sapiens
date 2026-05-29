using UnityEngine;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;

    private PlayerStats playerStats;

    public void Initialize(PlayerStats stats)
    {
        playerStats = stats;
    }

    public void Refresh()
    {
        if (playerStats == null || statsText == null) return;

        statsText.text =
$@"<b>Primary Stats</b>
STR: {playerStats.strength.Value:F0}  AGI: {playerStats.agility.Value:F0}
INT: {playerStats.intelligence.Value:F0}  VIT: {playerStats.vitality.Value:F0}

<b>Combat</b>
HP: {playerStats.currentHealth:F0} / {playerStats.finalMaxHealth:F0}
DMG: {playerStats.finalDamage:F1}  Crit: {playerStats.finalCritRate * 100:F1}%
CritDmg: {playerStats.finalCritDamage * 100:F0}%

<b>Defense</b>
Armor: {playerStats.finalArmor:F0}  Dodge: {playerStats.finalDodge * 100:F1}%
MagicResist: {playerStats.finalMagicResist * 100:F1}%

<b>Magic</b>
Fire: {playerStats.finalFireDamage:F0}  Ice: {playerStats.finalIceDamage:F0}
Lightning: {playerStats.finalLightningDamage:F0}";
    }
}
