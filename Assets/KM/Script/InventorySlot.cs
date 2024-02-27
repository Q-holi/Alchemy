using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Item item;                  // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private void Awake()
    {
        coverImage.gameObject.SetActive(false);
    }
    // 아이템 정보 초기화
    public void itemInit(Item info)
    {
        item = info;
        iconImage.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(item.texture2DImagePath);
        itemCount.text = item.count.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        coverImage.gameObject.SetActive(true);
        UIManager.Instance.SelectItem = item;
        UIManager.Instance.ItemSelected();
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중
    {
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝(해당 스크립트가 포함된 오브젝트에서 호출)
    {
        coverImage.gameObject.SetActive(false);
    }
}
