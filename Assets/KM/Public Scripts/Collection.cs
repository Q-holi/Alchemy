using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collection : Item
{
    public Vector4 options;

    public Collection(BaseCollectionData data) : base(data)
    {
        options = data.options;
    }
}
