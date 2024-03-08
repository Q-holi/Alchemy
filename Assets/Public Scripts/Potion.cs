using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPotion : BasePotion
{
    public NormalPotion() 
    {
        this.inventoryIndexNumber = 10;
        this.keyValue = 0;
        this.name = "Normal_Potion";
        this.rating = Rating.Normal;
        this.texture2DImagePath = "";
        this.count = 10;
    }

    public override void PotionEffect()
    {
        Debug.Log("일반적인 포션입니다.");
    }
}
