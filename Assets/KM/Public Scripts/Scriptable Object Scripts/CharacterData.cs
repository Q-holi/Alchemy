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

[System.Serializable]
[CreateAssetMenu(fileName = "Battle Data", menuName = "Battle System/Status Data", order = 1)]
public class CharacterData
{
    public Status defaultStatus;
    public Sprite sprite;
}
