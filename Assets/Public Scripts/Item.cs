using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Rating
{
    Normal,
    Rare,
    Epic,
    Legend
}
[Serializable]
public class Item 
{
    //public int inventoryIndexNumber;
    //public int InventoryIndexNumber { get => inventoryIndexNumber; set => inventoryIndexNumber = value; }

    public int keyvalue;
    //public int Keyvalue { get => keyvalue; set => keyvalue = value; }

    public string name;
    //public string Name { get => name; set => name = value; }

     public Rating rating;
    //public Rating Rating { get => rating; set => rating = value; }

    public string texture2DImagePath;
    //public string Texture2DImagePath { get => texture2DImagePath; set => texture2DImagePath = value; }

    public int count;
    //public int Count { get => count; set => count = value; }

}
