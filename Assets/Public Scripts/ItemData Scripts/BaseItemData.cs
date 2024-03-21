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

public class BaseItemData : ScriptableObject
{
    public int keyCode;
    public string itemName;
    public Sprite sprite;
    public Rating rating;
    public string detail;
    public int count;
}