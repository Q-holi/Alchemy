using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Collection iteminfo;           // 복사된 아이템 데이터
    [SerializeField] private SpriteRenderer itemIcon;       // 아이템 스프라이트
    [SerializeField] private Rigidbody2D itemRigidbody;     // 아이템 중력 조절용 rigidbody 컴포넌트
    public Rigidbody2D ItemRigidbody { get => itemRigidbody; set => itemRigidbody = value; }

    public void SetItemIcon(Collection item) // 아이템 정보 설정
    {
        iteminfo = item;
        itemIcon = this.GetComponent<SpriteRenderer>();
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(item.Texture2DImagePath);
        itemRigidbody.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        switch (collision.name)
        {
            case "UseIngredientArea": // 아이템 사용시
                if (AlchemyManager.instance.CaulDron.GetComponent<CaulDron>().UpdateContent(iteminfo))
                    Debug.Log("Use Item : " + iteminfo.Name);
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
