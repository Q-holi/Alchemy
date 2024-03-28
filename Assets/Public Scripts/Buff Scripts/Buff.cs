using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    private string name;    // 디버프 이름
    private bool isBuff;    // 버프 유형

    private int duration;   // 지속시간
    private int amount;     // 강도? 피해량?

    public virtual void Effect()
    {
        Debug.Log("This is None Effect Buff");
    }
}
