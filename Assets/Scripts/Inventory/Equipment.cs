using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<Inventory>();
        if (playerStats == null) playerStats = GetComponentInParent<PlayerStats>();
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnEquipmentChanged += RefreshAllModifiers;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnEquipmentChanged -= RefreshAllModifiers;
    }

    public void EquipFromSlot(int slotIndex)
    {
        if (inventory == null || playerStats == null) return;
        var slots = inventory.EquipmentSlots;
        if (slotIndex < 0 || slotIndex >= slots.Count) return;

        EquipmentData data = slots[slotIndex];

        // 卸下旧装备的 modifier
        EquipmentData old = inventory.GetEquipped(data.equipmentType);
        if (old != null) RemoveModifiers(old);

        inventory.Equip(data);
        ApplyModifiers(data);
    }

    public void UnequipSlot(EquipmentType slotType)
    {
        if (inventory == null) return;
        EquipmentData data = inventory.GetEquipped(slotType);
        if (data != null) RemoveModifiers(data);
        inventory.Unequip(slotType);
    }

    private void ApplyModifiers(EquipmentData data)
    {
        if (data == null || playerStats == null || data.modifiers == null) return;
        foreach (var mod in data.modifiers)
        {
            Stat stat = GetStat(mod.statType);
            if (stat != null)
                stat.AddModifier(new StatModifier(mod.modifierType, mod.value, data));
        }
        playerStats.RecalculateStats();
    }

    private void RemoveModifiers(EquipmentData data)
    {
        if (data == null || playerStats == null) return;
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            Stat stat = GetStat(statType);
            if (stat != null)
                stat.RemoveAllModifiersFromSource(data);
        }
        playerStats.RecalculateStats();
    }

    private void RefreshAllModifiers()
    {
        if (playerStats == null) return;
        // 清除所有装备 modifier，重新应用
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            Stat stat = GetStat(statType);
            if (stat != null)
                stat.ClearModifiers();
        }
        foreach (var kvp in inventory.EquippedDict)
            ApplyModifiers(kvp.Value);
    }

    private Stat GetStat(StatType type)
    {
        if (playerStats == null) return null;
        switch (type)
        {
            case StatType.Strength: return playerStats.strength;
            case StatType.Agility: return playerStats.agility;
            case StatType.Intelligence: return playerStats.intelligence;
            case StatType.Vitality: return playerStats.vitality;
            case StatType.Damage: return playerStats.damage;
            case StatType.CritRate: return playerStats.critRate;
            case StatType.CritDamage: return playerStats.critDamage;
            case StatType.MaxHealth: return playerStats.maxHealth;
            case StatType.Armor: return playerStats.armor;
            case StatType.Dodge: return playerStats.dodge;
            case StatType.MagicResist: return playerStats.magicResist;
            case StatType.FireDamage: return playerStats.fireDamage;
            case StatType.IceDamage: return playerStats.iceDamage;
            case StatType.LightningDamage: return playerStats.lightningDamage;
            default: return null;
        }
    }
}
