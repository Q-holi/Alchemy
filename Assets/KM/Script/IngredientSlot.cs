using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class IngredientSlot : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler
{
    [SerializeField] private Collection item;            // 아이템 정보
    [SerializeField] private Image iconImage;            // 아이템 이미지
    [SerializeField] private Image itemFrame;            // 아이템 프레임
    [SerializeField] private TextMeshProUGUI itemCount;  // 아이템 갯수

    [SerializeField] private Image coverImage;           // MouseOver 할때 강조효과

    private GameObject selectItem;  // 드래그시, 복사된 아이템

    private void Awake()
    {
        coverImage.gameObject.SetActive(false);
    }

    // 아이템 정보 초기화
    public void ItemInit(Collection info)
    {
        item = info;
        iconImage.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(item.Texture2DImagePath);
        itemCount.text = item.Count.ToString();
        itemFrame.color = AlchemyManager.instance.GetColor(item.rating);
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작시
    {
        AlchemyManager.instance.SelectItem = item;
        coverImage.gameObject.SetActive(true);

        selectItem = Instantiate(AlchemyManager.instance.SelectItemPrefab,
        AlchemyManager.instance.ScreenToWorldPos(), Quaternion.identity);
        selectItem.GetComponent<SelectItem>().SetItemIcon(item);
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중
    {
        selectItem.transform.position = AlchemyManager.instance.ScreenToWorldPos();
    }

    public void OnEndDrag(PointerEventData eventData)   // 드래그 끝 (해당 스크립트가 포함된 오브젝트에서 호출)
    {
        selectItem.GetComponent<SelectItem>().ItemRigidbody.gravityScale = 10;

        GameObject temp = AlchemyManager.instance.Detectray();
        if (temp)
        {
            if (temp.name == "IngredientList") // 재료창 위에서 드래그가 끝났으면, 아이템 사용 취소
            {
                Debug.Log("Item Use Cancel");
                Destroy(selectItem);
            }
        }

        coverImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 올렸을때
    {
        AlchemyManager.instance.SelectItem = item;
        coverImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 빠졌을때
    {
        coverImage.gameObject.SetActive(false);
    }
}
