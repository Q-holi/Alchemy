using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : IItem
{
    public int InventoryIndexNumber { get; set; }
    public int KeyValue { get; set; }
    public string Name { get; set; }
    public Rating Rating { get; set; }
    public string Texture2DImagePath { get; set; }
    public int Count { get; set; }

    public abstract void PotionEffect();
}

// 데코레이터
public abstract class PotionEnchant : Potion
{
    protected Potion potion;

    public override void PotionEffect()
    {
        potion.PotionEffect();
    }
}
