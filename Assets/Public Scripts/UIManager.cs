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
    [SerializeField] private Transform inventoryArea;   // 인벤토리 영역
    [SerializeField] private Collider2D cauldronArea;    // 가마솥 영역
    [SerializeField] private Transform itemInfo;        // 아이템 정보
    [SerializeField] private InventoryData itemData;    // 인벤토리 아이템 데이터

    private GameObject tempImage;   // 복사될 오브젝트
    [SerializeField] private Collection selectItem;        // 선택된 아이템

    private void Awake()
    {
        ShowItemList();
    }

    public Collection SelectItem
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

    // 사용 가능한 아이템 리스트 출력
    public void ShowItemList()
    {
        GameObject slotPrefab = Resources.Load<GameObject>("IngredientSlot");

        foreach (Collection data in itemData.playerData.collection)
        {
            if (data.Keyvalue > 2000)
            {
                GameObject obj = Instantiate(slotPrefab, inventoryArea);
                obj.GetComponent<InventorySlot>().itemInit(data);
            }
        }
    }

    // 사용하려는 아이템 정보 출력
    public void ShowItemInfo()
    {
        itemInfo.GetComponent<ItemInfo>().ShowItemInfo(selectItem);
    }

    // 아이템 선택되었을때
    public void ItemSelected()
    {
        GameObject obj = Resources.Load<GameObject>("SelectItem");
        obj.GetComponent<SelectItem>().SetItemIcon(
            Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(selectItem.Texture2DImagePath));
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
