using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D itemRigidbody;     // 아이템 중력 조절용 rigidbody 컴포넌트
    [SerializeField] private GameObject uiIcon;             // 드래그될 아이템 이미지

    private int itemKey; // 복사된 아이템
    private bool isDragging = false;

    public int GetIteminfo { get => itemKey; }

    public void OnEnable()
    {
        InventoryEventHandler.OnItemDragging += SetItemIcon;
    }

    public void OnDestroy()
    {
        InventoryEventHandler.OnItemDragging -= SetItemIcon;
    }

    public void SetItemIcon(int keyCode, bool dragging) // 아이템 정보 설정
    {
        itemKey = keyCode;
        isDragging = dragging;
        uiIcon.GetComponent<Image>().sprite = InventoryManager.itemDB[itemKey].sprite;
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        itemRigidbody.gravityScale = 0;
    }

    private void Update()
    {
        // ui 아이콘 이미지 위치 설정
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        if (isDragging)
        {
            // 실제 상호작용할 오브젝트의 위치 설정 (유니티에서 UI 충돌은 없다.)
            gameObject.transform.position = UtilFunction.ScreenToWorldPos();
        }
        else
        {
            itemRigidbody.gravityScale = 10;
        }
    }
}
