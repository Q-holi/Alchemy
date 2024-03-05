using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Collection iteminfo;
    [SerializeField] private SpriteRenderer itemIcon;
    [SerializeField] private Rigidbody2D itemRigidbody;
    public Rigidbody2D ItemRigidbody { get => itemRigidbody; set => itemRigidbody = value; }

    public void SetItemIcon(Collection item)
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
            case "UseIngredientArea":
                Debug.Log("Use Item : " + iteminfo.Name);
                Destroy(this.gameObject);
                break;
            case "ItemCancel":
                Debug.Log("Item Use Cancel");
                Destroy(this.gameObject);
                break;
        }
    }
}
