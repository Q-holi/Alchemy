using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템
    private InventoryItem itemData;

    public int GetItem { get => itemData.itemCode; }

    private void Awake()
    {
        coverImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 슬롯 아이템 정보 초기화 및 업데이트
    /// </summary>
    public void ItemInit(ItemDetails item)
    {
        int index = InventoryManager.Instance.SearchItem(item.itemCode);
        itemData = InventoryManager.Instance.inventoryLists[index];

        iconImage.sprite = item.sprite;
        itemCount.text = InventoryManager.Instance.inventoryLists[index].itemQuantity.ToString();
        itemFrame.color = UtilFunction.GetColor(item.itemRating);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        InventoryManager.Instance.isDragging = true;
        if (itemData.itemQuantity <= 0)
            return;

        coverImage.gameObject.SetActive(true);

        // 드래그 아이템 복사본 생성
        selectItem = Instantiate(InventoryManager.Instance.SelectItemPrefab,
        UtilFunction.ScreenToWorldPos(), Quaternion.identity);
        // 아이템 정보 설정
        // 아이템을 생성한뒤 이벤트를 등록하므로, 반드시 복사본을 먼저 만들 것
        InventoryEventHandler.OnItemDragging(itemData.itemCode, InventoryManager.Instance.isDragging);
        InventoryEventHandler.OnUseItem(itemData.itemCode, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 아이템 드래그를 인식하려면 아무 기능이 없더라도 있어야 함
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        InventoryManager.Instance.isDragging = false;
        if (itemData.itemCode <= 0 || selectItem == null)
            return;

        InventoryEventHandler.OnItemDragging(itemData.itemCode, InventoryManager.Instance.isDragging);

        if (UtilFunction.Detectray(InventoryManager.Instance.gameObject.name))
        {
            InventoryEventHandler.OnUseItem(itemData.itemCode, false);
            Destroy(selectItem);
        }

        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        if (InventoryManager.Instance.isDragging)       // 드래그 중일땐 인식 X
            return;

        InventoryEventHandler.OnMouse?.Invoke(itemData.itemCode);
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        if (InventoryManager.Instance.isDragging)       // 드래그 중일땐 인식 X
            return;
        coverImage.gameObject.SetActive(false);
    }
}
