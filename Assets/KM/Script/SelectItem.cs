using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private Image itemIcon;

    public void SetItemIcon(Sprite sprite)
    {
        itemIcon = this.GetComponent<Image>();
        itemIcon.sprite = sprite;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
