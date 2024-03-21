using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryList : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹
    [SerializeField] private N_Inventory inventory;             // 인벤토리 데이터
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private Item selectItem;        // 선택된 아이템
    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    public bool isDragging = false;     // 아이템 드래그 감지

    #region GetSet
    public N_Inventory Inventory { get => inventory; }
    public GameObject SelectItemPrefab { get => selectItemPrefab; }
    public Item SelectItem { get => selectItem; set => selectItem = value; }
    #endregion

    public void InventoryInit(List<Item> data) // 인벤토리 데이터기반 아이템 설정
    {
        foreach (Item item in data)
        {
            if (item is Collection collection)
            {
                GameObject slot = Instantiate(slotPrefab, slotTransform);
                slot.GetComponent<InventorySlot>().ItemInit(collection);
                slotList.Add(slot);
            }   
        }
    }

    public void InventoryUpdate()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<InventorySlot>().ItemInit(inventory.items[i]);
        }
    }

    public void ItemUse(bool isUse, Collection item)
    {
        if (isUse)
        {
            inventory.items.Find(x => x == item).count--;
            InventoryUpdate();
        }
        else
        {
            inventory.items.Find(x => x == item).count++;
            InventoryUpdate();
        }
    }
}
