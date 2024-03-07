using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Inter_Rating
{
    Normal,
    Rare,
    Epic,
    Legend
}

public interface IItem
{
    int InventoryIndexNumber { get; set; }
    int KeyValue { get; set; }
    string Name { get; set; }
    Inter_Rating Rating { get; set; }
    string Texture2DImagePath { get; set; }
    int Count { get; set; }
}
