using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] public int itemkey;
    [SerializeField] public int count = 50;

    public Item(int keyCode) { itemkey = keyCode; }
}