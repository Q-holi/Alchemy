using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] public Item item;            // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템
    private InventoryManager inventory;

    private void Awake()
    {
        inventory = gameObject.GetComponentInParent<InventoryManager>();
        coverImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 슬롯 아이템 정보 초기화
    /// </summary>
    public void ItemInit(Item item)
    {
        // Item 에 count(갯수) 를 저장하기때문에 Item 형태로 받아와야 함
        this.item = item;
        iconImage.sprite = InventoryManager.itemDB[item.itemkey].sprite;
        itemCount.text = item.count.ToString();
        itemFrame.color = UtilFunction.GetColor(InventoryManager.itemDB[item.itemkey].rating);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        inventory.isDragging = true;
        if (item.count <= 0)
            return;

        coverImage.gameObject.SetActive(true);

        // 드래그 아이템 복사본 생성
        selectItem = Instantiate(inventory.SelectItemPrefab,
        UtilFunction.ScreenToWorldPos(), Quaternion.identity);
        // 아이템 정보 설정
        // 아이템을 생성한뒤 이벤트를 등록하므로, 반드시 복사본을 먼저 만들 것
        InventoryEventHandler.OnItemDragging(item.itemkey, inventory.isDragging);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 아이템 드래그를 인식하려면 아무 기능이 없더라도 있어야 함
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        inventory.isDragging = false;
        if (item.count <= 0 || selectItem == null)
            return;

        InventoryEventHandler.OnItemDragging(item.itemkey, inventory.isDragging);

        if (UtilFunction.Detectray(inventory.gameObject.name))
        {
            Debug.Log("Item Use Cancel");
            Destroy(selectItem);
        }

        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        if (inventory.isDragging)       // 드래그 중일땐 인식 X
            return;

        InventoryEventHandler.OnMouse?.Invoke(item);
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        if (inventory.isDragging)       // 드래그 중일땐 인식 X
            return;
        coverImage.gameObject.SetActive(false);
    }


}
