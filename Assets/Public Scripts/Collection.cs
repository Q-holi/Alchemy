using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Collection : IItem
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

    public int red_Option;
    public int Red_Option { get => red_Option; set => red_Option = value; }

    public int green_Option;
    public int Green_Option { get => green_Option; set => green_Option = value; }

    public int blue_Option;
    public int Blue_Option { get => blue_Option; set => blue_Option = value; }

    public int alpha_Option;
    public int Alpha_Option { get => alpha_Option; set => alpha_Option = value; }

    public Collection(IItem item, int red_Option, int black_Option, int blue_Option)
    {
        this.inventoryIndexNumber = item.InventoryIndexNumber;
        this.keyValue = item.KeyValue;
        this.name = item.Name;
        this.rating = item.Rating;
        this.texture2DImagePath = item.Texture2DImagePath;
        this.count = item.Count;

        this.red_Option = red_Option;
        this.green_Option = black_Option;
        this.blue_Option = blue_Option;
    }
}
