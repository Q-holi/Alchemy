
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionFactory : AbsItemCreator//-- 앞으로 채집물을 생성할 팩토리 클래스 
{
    public static CollectionFactory collectionFactory;
    // Start is called before the first frame update
    public override Collection CreateItem(Item _item)
    {
        Collection _collection = new Collection(_item,7, 5,1);
        return _collection;    
    }
}
