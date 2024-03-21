using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tool : Item
{
    public int durability;

    public Tool(BaseToolData data) : base(data)
    {
        durability = data.duration;
    }
}
