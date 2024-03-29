using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Battle Data", menuName = "Battle System/Buff Data", order = 1)]
public class Buff : ScriptableObject
{
    public string name;    // 버프 이름
    public bool isBuff;    // 버프 유형

    public int duration;   // 지속시간
    public int amount;     // 강도? 피해량?

    public virtual void Effect()
    {
        Debug.Log("This is None Effect Buff");
    }
}
