using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collection : Item
{
    public Vector4 options;

    public Collection(int keyCode) : base(keyCode)
    {
        if(InventoryManager.itemDB[keyCode] is BaseCollectionData collectionData)
            options = collectionData.options;
    }
}
