using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public static class ItemFactory
{
    public static N_Collection CreateCollection(IItem item, int red_Option, int black_Option, int blue_Option)
    {
        return new N_Collection(item, red_Option, black_Option, blue_Option);
    }
}


/*
 1. 팩토리 패턴으로 채집물(Collection)을 생성 해야한다. 
CreatItem 호출시 Return이 Collection으로 반환이 되어야 한다. 
또 다른 CreatItem 호풀시 Collection_Tool 으로 반환이 되어야 하는 것도 필요 

위 2가지 성공 시 포션이라는 클래스 생성 후 반환 또한 포션으로도 가능이 되어야 한다. 
 */