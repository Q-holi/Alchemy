using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff
{
    public string buffName;    // 버프 이름
    public bool isBuff;    // 버프 유형

    public int duration;   // 지속시간
    public int amount;     // 강도? 피해량?

    public virtual void Effect()
    {
        Debug.Log("This is None Effect Buff");
    }
}
