using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public struct ItemData
{
    public int location;
    public int itemCode;
}

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템
    private ItemData itemData;

    private void Awake()
    {
        coverImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 슬롯 아이템 정보 초기화 및 업데이트
    /// </summary>
    public void ItemInit(int itemCode)
    {
        itemData.itemCode = itemCode;

        iconImage.sprite = InventoryManager.Instance.GetItemDetails(itemCode).itemSprite;

        if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, itemCode) != -1)
        {
            itemCount.text = InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].
                                Find(x => x.itemCode == itemCode).itemQuantity.ToString();
            itemData.location = (int)InventoryLocation.player;
        }
        else
        {
            itemCount.text = InventoryManager.Instance.inventoryLists[(int)InventoryLocation.chest].
                                Find(x => x.itemCode == itemCode).itemQuantity.ToString();
            itemData.location = (int)InventoryLocation.chest;
        }

        itemFrame.color = UtilFunction.GetColor(0);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        testinventory.instance.isDragging = true;
        coverImage.gameObject.SetActive(true);

        // 드래그 아이템 복사본 생성
        selectItem = Instantiate(testinventory.instance.selectItemPrefab,
        UtilFunction.ScreenToWorldPos(), Quaternion.identity);
        // 아이템 정보 설정
        // 아이템을 생성한뒤 이벤트를 등록하므로, 반드시 복사본을 먼저 만들 것
        InventoryEventHandler.OnItemDragging(itemData.itemCode, testinventory.instance.isDragging);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 아이템 드래그를 인식하려면 아무 기능이 없더라도 있어야 함
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        testinventory.instance.isDragging = false;
        if (itemData.itemCode <= 0 || selectItem == null)
            return;

        InventoryEventHandler.OnItemDragging(itemData.itemCode, testinventory.instance.isDragging);

        if (UtilFunction.Detectray(gameObject.GetComponentInParent<GameObject>().name))
        {
            Destroy(selectItem);
        }

        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        if (testinventory.instance.isDragging)       // 드래그 중일땐 인식 X
            return;

        InventoryEventHandler.OnMouse?.Invoke(itemData.itemCode);
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        if (testinventory.instance.isDragging)       // 드래그 중일땐 인식 X
            return;
        coverImage.gameObject.SetActive(false);
    }
}
