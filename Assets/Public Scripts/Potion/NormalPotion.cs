using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPotion : Potion
{
    public NormalPotion(IItem item)
    {
        this.InventoryIndexNumber = item.InventoryIndexNumber;
        this.KeyValue = item.KeyValue;
        this.Name = item.Name;
        this.Rating = item.Rating;
        this.Texture2DImagePath = item.Texture2DImagePath;
        this.Count = item.Count;
    }
    public override void PotionEffect()
    {
        Debug.Log("아무 효과 없는 일반 포션");
    }
}
