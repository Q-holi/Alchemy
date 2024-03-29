using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Collection", menuName = "Inventory System/Collection", order = 1)]
public class BaseCollectionData : BaseItemData
{
    public int r;
    public int g;
    public int b;
    public int a;
}
