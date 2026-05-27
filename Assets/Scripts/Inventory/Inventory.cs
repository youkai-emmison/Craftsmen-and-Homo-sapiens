using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("背包容量（装备格子数）")]
    [SerializeField] private int equipmentCapacity = 20;

    [Header("武器槽位数量")]
    [SerializeField] private int maxWeaponSlots = 5;

    [Header("初始物品")]
    [SerializeField] private EquipmentData[] startingEquipment;
    [SerializeField] private MaterialData[] startingMaterials;

    // 装备背包（有容量上限）
    private readonly List<EquipmentData> equipmentSlots = new List<EquipmentData>();

    // 材料仓库（无容量上限，按类型堆叠）
    private readonly Dictionary<MaterialData, int> materialDict = new Dictionary<MaterialData, int>();

    // 已穿戴装备（单件类型：头盔、盔甲、饰品、药水）
    private readonly Dictionary<EquipmentType, EquipmentData> equippedDict = new Dictionary<EquipmentType, EquipmentData>();

    // 已装备武器（多个槽位）
    private readonly List<EquipmentData> equippedWeapons = new List<EquipmentData>();

    public event Action OnInventoryChanged;
    public event Action OnEquipmentChanged;

    public IReadOnlyList<EquipmentData> EquipmentSlots => equipmentSlots;
    public IReadOnlyDictionary<MaterialData, int> MaterialDict => materialDict;
    public IReadOnlyDictionary<EquipmentType, EquipmentData> EquippedDict => equippedDict;
    public IReadOnlyList<EquipmentData> EquippedWeapons => equippedWeapons;
    public int EquipmentCapacity => equipmentCapacity;
    public int MaxWeaponSlots => maxWeaponSlots;

    private void Awake()
    {
        LoadStartingItems();
    }

    private void LoadStartingItems()
    {
        if (startingEquipment != null)
        {
            foreach (var eq in startingEquipment)
                if (eq != null) AddEquipment(eq);
        }
        if (startingMaterials != null)
        {
            foreach (var mat in startingMaterials)
                if (mat != null) AddMaterial(mat, 1);
        }
    }

    // ── 装备操作 ──

    public bool AddEquipment(EquipmentData data)
    {
        if (data == null || equipmentSlots.Count >= equipmentCapacity) return false;
        equipmentSlots.Add(data);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveEquipment(EquipmentData data)
    {
        if (data == null) return false;
        bool removed = equipmentSlots.Remove(data);
        if (removed) OnInventoryChanged?.Invoke();
        return removed;
    }

    // ── 材料操作 ──

    public void AddMaterial(MaterialData data, int amount)
    {
        if (data == null || amount <= 0) return;
        if (materialDict.ContainsKey(data))
            materialDict[data] += amount;
        else
            materialDict[data] = amount;
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveMaterial(MaterialData data, int amount)
    {
        if (data == null || amount <= 0) return false;
        if (!materialDict.ContainsKey(data) || materialDict[data] < amount) return false;
        materialDict[data] -= amount;
        if (materialDict[data] <= 0) materialDict.Remove(data);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetMaterialCount(MaterialData data)
    {
        if (data == null) return 0;
        return materialDict.ContainsKey(data) ? materialDict[data] : 0;
    }

    // ── 穿戴操作（单件类型：头盔、盔甲、饰品、药水）──

    public bool Equip(EquipmentData data)
    {
        if (data == null) return false;

        // 同类型已穿戴 → 卸下回背包
        if (equippedDict.ContainsKey(data.equipmentType))
        {
            if (!Unequip(data.equipmentType)) return false;
        }

        equipmentSlots.Remove(data);
        equippedDict[data.equipmentType] = data;
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public bool Unequip(EquipmentType slotType)
    {
        if (!equippedDict.ContainsKey(slotType)) return false;
        if (equipmentSlots.Count >= equipmentCapacity) return false;

        EquipmentData data = equippedDict[slotType];
        equippedDict.Remove(slotType);
        equipmentSlots.Add(data);
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public EquipmentData GetEquipped(EquipmentType slotType)
    {
        return equippedDict.ContainsKey(slotType) ? equippedDict[slotType] : null;
    }

    // ── 武器槽位操作 ──

    public bool EquipWeapon(EquipmentData data, int slotIndex)
    {
        if (data == null || data.equipmentType != EquipmentType.Weapon) return false;
        if (slotIndex < 0 || slotIndex >= maxWeaponSlots) return false;

        // 该槽位已有武器 → 卸下
        if (slotIndex < equippedWeapons.Count && equippedWeapons[slotIndex] != null)
        {
            if (!UnequipWeapon(slotIndex)) return false;
        }

        equipmentSlots.Remove(data);

        // 补齐列表
        while (equippedWeapons.Count <= slotIndex)
            equippedWeapons.Add(null);

        equippedWeapons[slotIndex] = data;
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public bool UnequipWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equippedWeapons.Count) return false;
        if (equippedWeapons[slotIndex] == null) return false;
        if (equipmentSlots.Count >= equipmentCapacity) return false;

        EquipmentData data = equippedWeapons[slotIndex];
        equippedWeapons[slotIndex] = null;
        equipmentSlots.Add(data);
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public EquipmentData GetEquippedWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= equippedWeapons.Count) return null;
        return equippedWeapons[slotIndex];
    }

    // ── 丢弃 ──

    public void DiscardEquipment(EquipmentData data)
    {
        if (data == null) return;
        if (equipmentSlots.Remove(data))
            OnInventoryChanged?.Invoke();
    }
}
