using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Item item;
    private SpriteRenderer spriteRender;

    // 아이템 초기화
    public void itemInit(Item info)
    {
        item = info;
    }
}
