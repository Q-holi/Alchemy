using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public abstract class AbsItemFactory
{ 
    public abstract Item CreateItem();
    //public abstract Collection CreateCollection(int _red_Option, int _black_Option, int _blue_Option);

    //public abstract CollectionTool CreateCollectionTool();
}
