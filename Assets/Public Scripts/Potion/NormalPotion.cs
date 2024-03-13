using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPotion : Potion
{
    public NormalPotion()
    {
        this.InventoryIndexNumber = 0;
        this.KeyValue = 0;
        this.Name = "Normal Potion";
        this.Rating = Rating.Normal;
        this.Texture2DImagePath = "item_01";
        this.Count = 1;
    }
    public override void PotionEffect()
    {
        Debug.Log("아무 효과 없는 일반 포션");
    }
}
