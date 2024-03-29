using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Poison : Buff
{
    public Poison(int duration, int amount)
    {
        this.name = "Poison";
        this.isBuff = false;
        this.duration = duration;
        this.amount = amount;
    }

    public override void Effect()
    {
        Debug.Log(duration + "턴 동안, " + amount + "만큼 지속피해");
    }
}
