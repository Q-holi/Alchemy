using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    [SerializeField] public BaseItemData itemData;
    [SerializeField] public int count = 50;

    public Item(BaseItemData data) { itemData = data; }

    public void SetAmount(int amount) { count = amount; }
}