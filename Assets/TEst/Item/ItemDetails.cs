using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[System.Serializable]

public class ItemDetails
{
    public int itemCode;
    public string name;
    public Sprite sprite;
    public string detail;
    public ItemRating itemRating;
    public bool potion;
    public bool collection;
    public bool tool;
    public List<int> itmeOption = new List<int>();
}