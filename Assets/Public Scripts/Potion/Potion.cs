using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Potion : IItem
{
    public int inventroyIndexNumber;
    public int InventoryIndexNumber { get; set; }

    public int keyValue;
    public int KeyValue { get; set; }

    public string name;
    public string Name { get; set; }

    public Rating rating;
    public Rating Rating { get; set; }

    public string texture2DImagePath;
    public string Texture2DImagePath { get; set; }

    public int count;
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
