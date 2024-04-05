using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///충돌한 오브젝트의 Collider2D collision의 정보를 가져온다.   Item item = collision.GetComponent<Item>();
///그렇게 충돌한 오브젝트가 있을시 그 아이템의 상세정보를 가져 온다.  ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
///반환받은 상세 정보 아이템이 드랍이 가능한 아이템이면 인벤토리에 충돌한 아이템 정보를 넘겨준다. 
///</summary>
public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //--충돌한 아이템을 가져와서 인벤토리 매니저에 있는 아이템정보를 불러온다.
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            //--충돌한 아이템중 상세 정보를 불러오기
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
          

            //--상세 정보중 픽업이 가능한 아이템이면 그 아이템을 인벤토리에 추가한다.
            if (itemDetails.canBePickedUp == true)
                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);
        }
    }
}
