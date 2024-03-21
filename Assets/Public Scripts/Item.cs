using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Item
{
    public BaseItemData itemData;
    public int count;

    public Item(BaseItemData data) { itemData = data; }

    public void SetAmount(int amount) { count = amount; }
}