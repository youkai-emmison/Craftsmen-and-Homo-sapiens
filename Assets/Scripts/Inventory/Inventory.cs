using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("背包容量（装备格子数）")]
    [SerializeField] private int equipmentCapacity = 20;

    [Header("初始物品")]
    [SerializeField] private EquipmentData[] startingEquipment;
    [SerializeField] private MaterialData[] startingMaterials;

    // 装备背包（有容量上限）
    private readonly List<EquipmentData> equipmentSlots = new List<EquipmentData>();

    // 材料仓库（无容量上限，按类型堆叠）
    private readonly Dictionary<MaterialData, int> materialDict = new Dictionary<MaterialData, int>();

    // 已穿戴装备
    private readonly Dictionary<EquipmentType, EquipmentData> equippedDict = new Dictionary<EquipmentType, EquipmentData>();

    public event Action OnInventoryChanged;
    public event Action OnEquipmentChanged;

    public IReadOnlyList<EquipmentData> EquipmentSlots => equipmentSlots;
    public IReadOnlyDictionary<MaterialData, int> MaterialDict => materialDict;
    public IReadOnlyDictionary<EquipmentType, EquipmentData> EquippedDict => equippedDict;
    public int EquipmentCapacity => equipmentCapacity;

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

    // ── 穿戴操作 ──

    public void Equip(EquipmentData data)
    {
        if (data == null) return;

        // 同类型已穿戴 → 卸下回背包
        if (equippedDict.ContainsKey(data.equipmentType))
            Unequip(data.equipmentType);

        equipmentSlots.Remove(data);
        equippedDict[data.equipmentType] = data;
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
    }

    public void Unequip(EquipmentType slotType)
    {
        if (!equippedDict.ContainsKey(slotType)) return;

        EquipmentData data = equippedDict[slotType];
        if (equipmentSlots.Count >= equipmentCapacity) return; // 背包满

        equippedDict.Remove(slotType);
        equipmentSlots.Add(data);
        OnInventoryChanged?.Invoke();
        OnEquipmentChanged?.Invoke();
    }

    public EquipmentData GetEquipped(EquipmentType slotType)
    {
        return equippedDict.ContainsKey(slotType) ? equippedDict[slotType] : null;
    }

    // ── 丢弃 ──

    public void DiscardEquipment(EquipmentData data)
    {
        if (data == null) return;
        if (equipmentSlots.Remove(data))
            OnInventoryChanged?.Invoke();
    }
}
