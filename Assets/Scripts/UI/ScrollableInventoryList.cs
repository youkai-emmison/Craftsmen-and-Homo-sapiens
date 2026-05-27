using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        tooltip = GetComponentInParent<InventoryPanel>()?.GetComponentInChildren<ItemTooltip>(true);
    }

    /// <summary>
    /// 刷新列表内容。
    /// </summary>
    public void Refresh(Inventory inventory, Action<int> onClick, Action<int> onDiscard)
    {
        if (inventory == null || contentParent == null || itemPrefab == null)
        {
            Debug.LogWarning($"ScrollableInventoryList.Refresh 跳过: inventory={inventory != null}, contentParent={contentParent != null}, itemPrefab={itemPrefab != null}");
            return;
        }

        var equipmentList = inventory.EquipmentSlots;
        var materialDict = inventory.MaterialDict;

        int totalItems = equipmentList.Count + materialDict.Count;

        // 确保有足够的列表项
        while (spawnedItems.Count < totalItems)
        {
            InventoryListItem newItem = Instantiate(itemPrefab, contentParent);
            spawnedItems.Add(newItem);
            Debug.Log($"ScrollableInventoryList: 创建新 item #{spawnedItems.Count}, active={newItem.gameObject.activeSelf}");
        }

        // 填充装备
        int idx = 0;
        for (int i = 0; i < equipmentList.Count; i++, idx++)
        {
            spawnedItems[idx].gameObject.SetActive(true);
            spawnedItems[idx].Setup(idx, equipmentList[i], onClick, onDiscard, tooltip);
            Debug.Log($"ScrollableInventoryList: 装备 [{idx}] {equipmentList[i].itemName}, icon={equipmentList[i].icon != null}");
        }

        // 填充材料
        foreach (var kvp in materialDict)
        {
            spawnedItems[idx].gameObject.SetActive(true);
            spawnedItems[idx].Setup(idx, kvp.Key, kvp.Value, onClick, onDiscard, tooltip);
            Debug.Log($"ScrollableInventoryList: 材料 [{idx}] {kvp.Key.itemName} x{kvp.Value}");
            idx++;
        }

        // 隐藏多余的列表项
        for (int i = idx; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].gameObject.SetActive(false);
        }

        Debug.Log($"ScrollableInventoryList.Refresh 完成: total={totalItems}, spawned={spawnedItems.Count}");
    }
}
