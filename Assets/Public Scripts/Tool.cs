using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool : Item
{
    public int durability;

    public Tool(int keyCode) : base(keyCode) 
    {
        if (InventoryManager.itemDB[keyCode] is BaseToolData toolData)
        {
            durability = toolData.durability;
        }
    }
}
