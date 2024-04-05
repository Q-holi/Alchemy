using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class ItemDetails
{
    public int itemCode;
    public ItemType itemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius;
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickedUp;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;
    public ItemRating itemRating;
    public bool potion;
    public bool collection;
    public bool tool;
    public List<int> itemOption = new List<int>();
}

