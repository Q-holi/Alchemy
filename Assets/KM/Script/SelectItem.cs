using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D itemRigidbody;     // 아이템 중력 조절용 rigidbody 컴포넌트
    [SerializeField] private GameObject uiIcon;             // 드래그될 아이템 이미지
    private IItem item; // 복사된 아이템의 정보

    public Rigidbody2D ItemRigidbody { get => itemRigidbody; set => itemRigidbody = value; }
    public IItem GetIteminfo { get => item; }

    public void SetItemIcon(IItem item) // 아이템 정보 설정
    {
        this.item = item;
        SpriteAtlas icons = Resources.Load<SpriteAtlas>("TempIcons");
        uiIcon.GetComponent<Image>().sprite = icons.GetSprite(item.Texture2DImagePath);
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        itemRigidbody.gravityScale = 0;
    }

    private void Update()
    {
        // ui 아이콘 이미지 위치 설정
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }
}
