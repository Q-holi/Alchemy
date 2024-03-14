using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] private IItem item;            // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템
    private InventoryList inventory;

    private void Awake()
    {
        inventory = gameObject.GetComponentInParent<InventoryList>();
        coverImage.gameObject.SetActive(false);
    }

    // 아이템 정보 초기화
    public void ItemInit(IItem info)
    {
        item = info;
        iconImage.sprite = Resources.Load<SpriteAtlas>("TempIcons").GetSprite(item.Texture2DImagePath);
        itemCount.text = item.Count.ToString();
        itemFrame.color = UtilFunction.GetColor(item.Rating);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        inventory.isDragging = true;
        if (item.Count <= 0)
            return;

        // 아이템 정보 넘겨주기
        inventory.SelectItem = item;
        coverImage.gameObject.SetActive(true);

        // 드래그 아이템 복사본 생성
        selectItem = Instantiate(inventory.SelectItemPrefab,
        UtilFunction.ScreenToWorldPos(), Quaternion.identity);
        // 아이템 정보 설정 << 다시 생각해보기
        selectItem.GetComponent<SelectItem>().SetItemIcon((Collection)item);
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중
    {
        if (item.Count <= 0 || selectItem == null)
            return;

        selectItem.transform.position = UtilFunction.ScreenToWorldPos();
        // 아이템 정보 넘겨주기
        inventory.SelectItem = item;
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        inventory.isDragging = false;
        if (item.Count <= 0 || selectItem == null)
            return;

        selectItem.GetComponent<SelectItem>().ItemRigidbody.gravityScale = 10;

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
        inventory.SelectItem = item;
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        if (inventory.isDragging)
            return;
        coverImage.gameObject.SetActive(false);
    }
}
