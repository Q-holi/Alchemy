using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private Collider2D inventoryArea;   // 인벤토리 영역
    [SerializeField] private Collider2D caulDronArea;    // 가마솥 영역

    private GameObject tempImage;   // 복사될 오브젝트
    private Item selectItem;        // 선택된 아이템

    public Item SelectItem
    {
        get { return selectItem; }
        set { selectItem = value; }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void ItemSelected()
    {
        GameObject obj = Resources.Load<GameObject>("SelectItem");
        obj.GetComponent<SelectItem>().SetItemIcon(
            Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(selectItem.texture2DImagePath));
        tempImage = Instantiate(obj, uiCanvas);
    }
}
