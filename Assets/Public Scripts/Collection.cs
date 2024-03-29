using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collection : Item
{
    public int r;
    public int g;
    public int b;
    public int a;
  

    public Collection(int keyCode) : base(keyCode)
    {
        if(InventoryManager.itemDB[keyCode] is BaseCollectionData collectionData)
        {
            r = collectionData.r;
            g = collectionData.g;
            b = collectionData.b;
            a = collectionData.a;
        }

    }
}
