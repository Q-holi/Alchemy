using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePotion : IItem
{
    public int inventoryIndexNumber;
    public int InventoryIndexNumber { get => inventoryIndexNumber; set => inventoryIndexNumber = value; }

    public int keyValue;
    public int KeyValue { get => keyValue; set => keyValue = value; }

    public string name;
    public string Name { get => name; set => name = value; }

    public Rating rating;
    public Rating Rating { get => rating; set => rating = value; }

    public string texture2DImagePath;
    public string Texture2DImagePath { get => texture2DImagePath; set => texture2DImagePath = value; }

    public int count;
    public int Count { get => count; set => count = value; }

    public abstract void PotionEffect();
}

public abstract class Decorater : BasePotion
{
    protected BasePotion potion = null;
}
