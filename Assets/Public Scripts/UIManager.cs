using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System;

[System.Serializable]
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private Collider2D inventoryArea;   // 인벤토리 영역
    [SerializeField] private Collider2D cauldronArea;    // 가마솥 영역
    [SerializeField] private Transform itemInfo;        // 아이템 정보

    private GameObject tempImage;   // 복사될 오브젝트
    [SerializeField] private Item selectItem;        // 선택된 아이템


    public Item SelectItem
    {
        get { return selectItem; }
        set { selectItem = value; }
    }

    public Collider2D InventoryArea
    {
        get { return InventoryArea; }
    }

    public Collider2D CauldronArea
    {
        get { return cauldronArea; }
    }

    public void ShowItemInfo()
    {
        itemInfo.GetComponent<ItemInfo>().ShowItemInfo(selectItem);
    }

    public void ItemSelected()
    {
        GameObject obj = Resources.Load<GameObject>("SelectItem");
        obj.GetComponent<SelectItem>().SetItemIcon(
            Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(selectItem.texture2DImagePath));
        tempImage = Instantiate(obj, uiCanvas);
    }

    public void ItemDragging()
    {
        tempImage.transform.position = Input.mousePosition;
    }

    public void ItemDrop()
    {
        tempImage.GetComponent<SelectItem>().ItemRigidbody.gravityScale = 300;
    }
}
