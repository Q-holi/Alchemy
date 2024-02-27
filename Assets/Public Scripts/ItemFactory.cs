using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ItemFactory : AbsItemFactory
{
    public override Item CreatItems()
    {
        Item item = new Item();
        return item;    
    }

}
