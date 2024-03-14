using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Collection iteminfo;           // 복사된 아이템 데이터
    [SerializeField] private Rigidbody2D itemRigidbody;     // 아이템 중력 조절용 rigidbody 컴포넌트
    [SerializeField] private GameObject uiIcon;
    public Rigidbody2D ItemRigidbody { get => itemRigidbody; set => itemRigidbody = value; }

    public void SetItemIcon(Collection item) // 아이템 정보 설정
    {
        iteminfo = item;
        uiIcon.GetComponent<Image>().sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(item.Texture2DImagePath);
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        itemRigidbody.gravityScale = 0;
    }

    public void Update()
    {
        // ui 아이콘 이미지 위치 설정
        uiIcon.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    //// 화면밖으로 아이콘이 나갔을때
    //private void OnBecameInvisible()
    //{
    //    Debug.Log("Item Use Cancel");
    //    AlchemyManager.instance.ItemUse(false, iteminfo);
    //    Destroy(this.gameObject);
    //}

    // 콜라이더와 충돌시
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        switch (collision.name)
        {
            case "UseIngredientArea": // 아이템 사용시
                if (AlchemyManager.instance.CaulDron.GetComponent<CaulDron>().UpdateContent(iteminfo))
                { 
                    Debug.Log("Use Item : " + iteminfo.Name);
                    AlchemyManager.instance.ItemUse(true, iteminfo);
                }
                else // 아이템 사용 실패
                { 
                    Debug.Log("Use Item False");
                }
                Destroy(this.gameObject);
                break;
            case "ItemCancel":  // 아이템 사용 취소 시
                Debug.Log("Item Use Cancel");
                Destroy(this.gameObject);
                break;
        }
    }
}
