using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可滚动的背包物品列表，显示所有装备和材料。
/// 需要挂在带有 ScrollRect 的物体上，Content 子物体需要 GridLayoutGroup。
/// </summary>
public class ScrollableInventoryList : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private InventoryListItem itemPrefab;

    private readonly List<InventoryListItem> spawnedItems = new List<InventoryListItem>();

    private ItemTooltip tooltip;

    private void Awake()
    {
        // 尝试从面板层级获取 tooltip
        tooltip = GetComponentInParent<InventoryPanel>()?.GetComponentInChildren<ItemTooltip>(true);
    }

    /// <summary>
    /// 刷新列表内容。
    /// </summary>
    public void Refresh(Inventory inventory, Action<int> onClick, Action<int> onDiscard)
    {
        if (inventory == null || contentParent == null || itemPrefab == null) return;

        var equipmentList = inventory.EquipmentSlots;
        var materialDict = inventory.MaterialDict;

        int totalItems = equipmentList.Count + materialDict.Count;

        // 确保有足够的列表项
        while (spawnedItems.Count < totalItems)
        {
            InventoryListItem newItem = Instantiate(itemPrefab, contentParent);
            spawnedItems.Add(newItem);
        }

        // 填充装备
        int idx = 0;
        for (int i = 0; i < equipmentList.Count; i++, idx++)
        {
            spawnedItems[idx].gameObject.SetActive(true);
            spawnedItems[idx].Setup(idx, equipmentList[i], onClick, onDiscard, tooltip);
        }

        // 填充材料
        foreach (var kvp in materialDict)
        {
            spawnedItems[idx].gameObject.SetActive(true);
            spawnedItems[idx].Setup(idx, kvp.Key, kvp.Value, onClick, onDiscard, tooltip);
            idx++;
        }

        // 隐藏多余的列表项
        for (int i = idx; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].gameObject.SetActive(false);
        }
    }
}
