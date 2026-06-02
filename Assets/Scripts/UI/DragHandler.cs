using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static DragHandler Instance { get; private set; }

    [SerializeField] private Image dragIconPrefab;

    private Image dragIconInstance;
    private Canvas parentCanvas;

    private void Awake()
    {
        Instance = this;
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragIconPrefab == null || parentCanvas == null) return;

        dragIconInstance = Instantiate(dragIconPrefab, parentCanvas.transform);
        dragIconInstance.raycastTarget = false;

        // 继承被拖拽物品的图标
        InventorySlotView slot = eventData.pointerDrag.GetComponent<InventorySlotView>();
        if (slot != null && slot.CurrentEquipmentData != null && slot.CurrentEquipmentData.icon != null)
        {
            dragIconInstance.sprite = slot.CurrentEquipmentData.icon;
            dragIconInstance.color = Color.white;
        }
        else if (slot != null && slot.CurrentMaterialData != null && slot.CurrentMaterialData.icon != null)
        {
            dragIconInstance.sprite = slot.CurrentMaterialData.icon;
            dragIconInstance.color = Color.white;
        }

        UpdateDragIconPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragIconPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null)
            Destroy(dragIconInstance.gameObject);
    }

    private void UpdateDragIconPosition(PointerEventData eventData)
    {
        if (dragIconInstance == null || parentCanvas == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            eventData.position,
            parentCanvas.worldCamera,
            out Vector2 localPos);
        dragIconInstance.rectTransform.anchoredPosition = localPos;
    }
}
