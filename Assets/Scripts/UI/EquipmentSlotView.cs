using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotView : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("装备类型")]
    [SerializeField] private EquipmentType slotType;

    [Header("武器槽位（仅当 slotType=Weapon 时使用）")]
    [SerializeField] private bool isWeaponSlot;
    [SerializeField] private int weaponSlotIndex;

    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image slotBackgroundImage;

    [Header("Colors")]
    [SerializeField] private Color emptyColor = new Color(0.2f, 0.2f, 0.25f, 0.8f);
    [SerializeField] private Color equippedColor = new Color(0.3f, 0.5f, 0.3f, 0.9f);

    public EquipmentType SlotType => slotType;
    public bool IsWeaponSlot => isWeaponSlot;
    public int WeaponSlotIndex => weaponSlotIndex;

    private Inventory inventory;
    private Equipment equipment;
    private ItemTooltip tooltip;

    private void Awake()
    {
        if (iconImage != null) iconImage.raycastTarget = false;
    }

    public void Initialize(Inventory inv, Equipment equip, ItemTooltip t)
    {
        inventory = inv;
        equipment = equip;
        tooltip = t;
    }

    public void Refresh()
    {
        if (inventory == null) return;

        EquipmentData data = isWeaponSlot
            ? inventory.GetEquippedWeapon(weaponSlotIndex)
            : inventory.GetEquipped(slotType);

        if (data != null)
        {
            SetBackground(equippedColor);
            if (iconImage != null)
            {
                iconImage.enabled = true;
                iconImage.sprite = data.icon;
                iconImage.color = data.icon != null ? Color.white : Color.gray;
            }
        }
        else
        {
            SetBackground(emptyColor);
            if (iconImage != null) iconImage.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventory == null || equipment == null) return;

        InventorySlotView draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotView>();
        if (draggedSlot == null || draggedSlot.CurrentEquipmentData == null) return;

        EquipmentData draggedData = draggedSlot.CurrentEquipmentData;

        if (isWeaponSlot)
        {
            if (draggedData.equipmentType != EquipmentType.Weapon) return;

            // 卸下旧武器（触发 RefreshAllModifiers）
            EquipmentData oldWeapon = inventory.GetEquippedWeapon(weaponSlotIndex);
            if (oldWeapon != null)
                equipment.UnequipWeaponSlot(weaponSlotIndex);

            // 装备新武器到指定槽位（触发 RefreshAllModifiers）
            inventory.EquipWeapon(draggedData, weaponSlotIndex);
        }
        else
        {
            if (draggedData.equipmentType != slotType) return;
            equipment.EquipFromSlot(draggedSlot.SlotIndex);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory == null || equipment == null) return;

        if (isWeaponSlot)
        {
            EquipmentData data = inventory.GetEquippedWeapon(weaponSlotIndex);
            if (data != null)
                equipment.UnequipWeaponSlot(weaponSlotIndex);
        }
        else
        {
            EquipmentData data = inventory.GetEquipped(slotType);
            if (data != null)
                equipment.UnequipSlot(slotType);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null || inventory == null) return;

        EquipmentData data = isWeaponSlot
            ? inventory.GetEquippedWeapon(weaponSlotIndex)
            : inventory.GetEquipped(slotType);

        if (data != null) tooltip.ShowEquipment(data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null) tooltip.Hide();
    }

    private void SetBackground(Color c)
    {
        if (slotBackgroundImage != null) slotBackgroundImage.color = c;
    }
}
