using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public struct Status
{
    public int hp;
    public int shield;
    public string name;
    public int atkPower;
    public int defPower;
    public int speed;
}

[System.Serializable]
public abstract class BattleCharacterData : ScriptableObject
{
    public Status defaultStatus;
    public Status currentStatus;
    public GameObject hpBar;
    public GameObject speedBar;
}
