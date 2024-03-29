using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Tool", menuName = "Inventory System/Tool", order = 3)]
public class BaseToolData : BaseItemData
{
    public int durability;
}
