using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("基础信息")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public ItemType itemType;
    [Range(0f, 1f)] public float dropRate = 0.5f;
}
