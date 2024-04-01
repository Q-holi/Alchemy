using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public struct Status
{
    public string name;
    public int hp;
    public int shield;
    public int atkPower;
    public int defPower;
    public int speed;
}

[CreateAssetMenu(fileName = "Battle Data", menuName = "Battle System/Status Data", order = 1)]
public class BattleCharacterData : ScriptableObject
{
    public Status defaultStatus;
    public Sprite sprite;
}
