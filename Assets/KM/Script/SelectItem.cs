using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D itemRigidbody;     // 아이템 중력 조절용 rigidbody 컴포넌트
    [SerializeField] private GameObject uiIcon;             // 드래그될 아이템 이미지

    private Item item; // 복사된 아이템의 정보
    private bool isDragging = false;

    public Rigidbody2D ItemRigidbody { get => itemRigidbody; set => itemRigidbody = value; }
    public Item GetIteminfo { get => item; }

    public void OnEnable()
    {
        InventoryEventHandler.OnItemDragging += SetItemIcon;
    }

    public void OnDestroy()
    {
        InventoryEventHandler.OnItemDragging -= SetItemIcon;
    }

    public void SetItemIcon(Item item, bool dragging) // 아이템 정보 설정
    {
        this.item = item;
        isDragging = dragging;
        uiIcon.GetComponent<Image>().sprite = item.itemData.sprite;
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        itemRigidbody.gravityScale = 0;
    }

    private void Update()
    {
        // ui 아이콘 이미지 위치 설정
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        if (isDragging)
        {
            // 실제 상호작용할 오브젝트 위치 설정
            gameObject.transform.position = UtilFunction.ScreenToWorldPos();
        }
        else
        {
            ItemRigidbody.gravityScale = 10;
        }
    }
}
