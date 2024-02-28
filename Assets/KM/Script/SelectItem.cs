using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Rigidbody2D itemRigidbody;

    public Rigidbody2D ItemRigidbody 
    {
        get { return itemRigidbody; }
        set { itemRigidbody = value; }
    }

    public void SetItemIcon(Sprite sprite)
    {
        itemIcon = this.GetComponent<Image>();
        itemIcon.sprite = sprite;
        itemRigidbody.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "UI")
        {
            Debug.Log("cauldron");
        }
    }
}
