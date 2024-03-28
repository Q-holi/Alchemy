using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public int amount;
    public int duration;

    public Potion(BasePotionData data) : base(data)
    {
        amount = data.amount;
        duration = data.duration;
    }
}
