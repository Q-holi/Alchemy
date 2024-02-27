using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Collection : Item
{
    public int red_Option;
    public int black_Option;
    public int blue_Option;
    public string Serialize()
    {
        // Item 클래스의 필드를 포함하여 직렬화
        string json = JsonUtility.ToJson(this);
        return json;
    }

}
