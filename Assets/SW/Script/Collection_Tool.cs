using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Collection_Tool : IItem
{
    // IItem 인터페이스 구현
    public int InventoryIndexNumber { get; set; }
    public int KeyValue { get; set; }
    public string Name { get; set; }
    public Rating Rating { get; set; }
    public string Texture2DImagePath { get; set; }
    public int Count { get; set; }
    public int Durability { get; set; }
    public int X_Range { get; set; }
    public int Y_Range { get; set; }

    public Collection_Tool(IItem item, int durability, int x_Range, int y_Range)
    {
        InventoryIndexNumber = item.InventoryIndexNumber;
        KeyValue = item.KeyValue;
        Name = item.Name;
        Rating = item.Rating;
        Texture2DImagePath = item.Texture2DImagePath;
        Count = item.Count;
        Durability = durability;
        X_Range = x_Range;
        Y_Range = y_Range;
    }
}