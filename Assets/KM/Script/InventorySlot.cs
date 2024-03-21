using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] private Item item;            // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템
    private Inventory inventory;

    private void Awake()
    {
        inventory = gameObject.GetComponentInParent<Inventory>();
        coverImage.gameObject.SetActive(false);
    }

    // 아이템 정보 초기화
    public void ItemInit(Item info)
    {
        item = info;
        iconImage.sprite = item.itemData.sprite;
        itemCount.text = item.count.ToString();
        itemFrame.color = UtilFunction.GetColor(item.itemData.rating);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        inventory.isDragging = true;
        if (item.count <= 0)
            return;

        // 아이템 정보 넘겨주기
        inventory.SelectItem = item;
        coverImage.gameObject.SetActive(true);

        // 드래그 아이템 복사본 생성
        selectItem = Instantiate(inventory.SelectItemPrefab,
        UtilFunction.ScreenToWorldPos(), Quaternion.identity);
        // 아이템 정보 설정
        EventHandler.OnItemDragging(item, true);
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중
    {
        if (item.count <= 0 || selectItem == null)
            return;

        // 아이템 정보 넘겨주기
        inventory.SelectItem = item;
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        inventory.isDragging = false;
        if (item.count <= 0 || selectItem == null)
            return;

        EventHandler.OnItemDragging(item, false);

        if (UtilFunction.Detectray(inventory.gameObject.name))
        {
            Debug.Log("Item Use Cancel");
            Destroy(selectItem);
        }

        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        if (inventory.isDragging)
            return;

        EventHandler.OnMouse?.Invoke(item);
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        if (inventory.isDragging)
            return;
        coverImage.gameObject.SetActive(false);
    }
}
