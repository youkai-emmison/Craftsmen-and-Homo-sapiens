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

    /// <summary>
    /// 从背包栏位穿戴装备到对应的装备槽。
    /// </summary>
    public void EquipFromSlot(int slotIndex)
    {
        if (inventory == null || playerStats == null) return;
        var slots = inventory.EquipmentSlots;
        if (slotIndex < 0 || slotIndex >= slots.Count) return;

        EquipmentData data = slots[slotIndex];

        if (data.equipmentType == EquipmentType.Weapon)
        {
            // 武器 → 找一个空的武器槽位
            int targetSlot = -1;
            for (int i = 0; i < inventory.MaxWeaponSlots; i++)
            {
                if (inventory.GetEquippedWeapon(i) == null) { targetSlot = i; break; }
            }
            if (targetSlot < 0) return; // 所有武器槽已满

            inventory.EquipWeapon(data, targetSlot);
            ApplyModifiers(data);
        }
        else
        {
            // 单件类型
            EquipmentData old = inventory.GetEquipped(data.equipmentType);
            if (old != null)
            {
                RemoveModifiers(old);
            }

            if (inventory.Equip(data))
            {
                ApplyModifiers(data);
            }
        }
    }

    /// <summary>
    /// 将已穿戴的装备从装备槽卸下回背包。
    /// </summary>
    public void UnequipSlot(EquipmentType slotType)
    {
        if (inventory == null) return;
        EquipmentData data = inventory.GetEquipped(slotType);
        if (data == null) return;

        RemoveModifiers(data);
        inventory.Unequip(slotType);
    }

    /// <summary>
    /// 将已装备的武器从武器槽卸下回背包。
    /// </summary>
    public void UnequipWeaponSlot(int slotIndex)
    {
        if (inventory == null) return;
        EquipmentData data = inventory.GetEquippedWeapon(slotIndex);
        if (data == null) return;

        RemoveModifiers(data);
        inventory.UnequipWeapon(slotIndex);
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
        if (playerStats == null || inventory == null) return;

        // 清除所有 modifier
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            Stat stat = GetStat(statType);
            if (stat != null)
                stat.ClearModifiers();
        }

        // 重新应用单件装备
        foreach (var kvp in inventory.EquippedDict)
            ApplyModifiers(kvp.Value);

        // 重新应用武器槽
        for (int i = 0; i < inventory.MaxWeaponSlots; i++)
        {
            EquipmentData weapon = inventory.GetEquippedWeapon(i);
            if (weapon != null) ApplyModifiers(weapon);
        }
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
