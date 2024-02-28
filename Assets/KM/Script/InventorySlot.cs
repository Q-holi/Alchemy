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
    [SerializeField] private Collection item;                  // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private void Awake()
    {
        coverImage.gameObject.SetActive(false);
    }

    // 아이템 정보 초기화
    public void itemInit(Collection info)
    {
        item = info;
        iconImage.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(item.Texture2DImagePath);
        itemCount.text = item.Count.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        UIManager.Instance.SelectItem = item;
        UIManager.Instance.ItemSelected();
        coverImage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중
    {
        UIManager.Instance.ItemDragging();
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝(해당 스크립트가 포함된 오브젝트에서 호출)
    {
        UIManager.Instance.ItemDrop();
        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        UIManager.Instance.SelectItem = item;
        UIManager.Instance.ShowItemInfo();
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        coverImage.gameObject.SetActive(false);
    }
}
