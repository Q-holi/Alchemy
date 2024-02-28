using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class ItemFactory : AbsItemFactory
{
    public override Item CreateItem()
    {
        return new Item();
    }

}
