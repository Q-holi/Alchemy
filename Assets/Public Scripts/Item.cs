using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
    [SerializeField] private int inventoryIndexNumber;
    public int InventoryIndexNumber { get => inventoryIndexNumber; set => inventoryIndexNumber = value; }

    [SerializeField] private int keyvalue;
    public int Keyvalue { get => keyvalue; set => keyvalue = value; }

    [SerializeField] private string name;
    public string Name { get => name; set => name = value; }

   [SerializeField] private Rating rating;
    public Rating Rating { get => rating; set => rating = value; }

    [SerializeField] private string texture2DImagePath;
    public string Texture2DImagePath { get => texture2DImagePath; set => texture2DImagePath = value; }

    [SerializeField] private int count;
    public int Count { get => count; set => count = value; }


    public Item(int _inventoryIndexNumber, int _keyvalue, string _name, Rating _rating, string _texture2DImagePath, int _count)
    {
        this.inventoryIndexNumber = _inventoryIndexNumber;
        this.keyvalue = _keyvalue;
        this.name = _name;
        this.rating = _rating;
        this.texture2DImagePath = _texture2DImagePath;
        this.count = _count;
    }

}
