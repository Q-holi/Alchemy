using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class N_Collection : IItem
{
    public int inventoryIndexNumber;
    public int InventoryIndexNumber { get => inventoryIndexNumber; set => inventoryIndexNumber = value; }

    public int keyValue;
    public int KeyValue { get => keyValue; set => keyValue = value; }

    public string name;
    public string Name { get => name; set => name = value; }

    public Inter_Rating rating;
    public Inter_Rating Rating { get => rating; set => rating = value; }

    public string texture2DImagePath;
    public string Texture2DImagePath { get => texture2DImagePath; set => texture2DImagePath = value; }

    public int count;
    public int Count { get => count; set => count = value; }

    public int red_Option;
    public int Red_Option { get => red_Option; set => red_Option = value; }

    public int black_Option;
    public int Black_Option { get => black_Option; set => black_Option = value; }

    public int blue_Option;
    public int Blue_Option { get => blue_Option; set => blue_Option = value; }

    public N_Collection(IItem item, int red_Option, int black_Option, int blue_Option)
    {
        this.inventoryIndexNumber = item.InventoryIndexNumber;
        this.keyValue = item.KeyValue;
        this.name = item.Name;
        this.rating = item.Rating;
        this.texture2DImagePath = item.Texture2DImagePath;
        this.count = item.Count;

        this.red_Option = red_Option;
        this.black_Option = black_Option;
        this.blue_Option = blue_Option;
    }
}