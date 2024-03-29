using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Potion", menuName = "Inventory System/Potion", order = 2)]
public class BasePotionData : BaseItemData
{
    public int amount;
    public int duration;
}

