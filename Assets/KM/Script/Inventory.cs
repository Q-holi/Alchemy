using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InventoryFilterType
{
    Collection,
    Potion,
    Tool
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹
    [SerializeField] private N_Inventory inventoryData;         // 인벤토리 데이터
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    private InventoryFilterType inventoryFilter;    // 인벤토리 아이템 유형 필터
    public bool isDragging = false;     // 아이템 드래그 감지

    #region GetSet
    public N_Inventory InventoryData { get => inventoryData; }
    public GameObject SelectItemPrefab { get => selectItemPrefab; }
    #endregion

    private void OnEnable()
    {
        InventoryEventHandler.OnUseItem += ItemUse;
        InventoryEventHandler.OnUseFilter += InentorySorting;
    }

    private void OnDestroy()
    {
        InventoryEventHandler.OnUseItem -= ItemUse;
        InventoryEventHandler.OnUseFilter -= InentorySorting;
    }

    // 인벤토리 데이터로 인벤토리 정보 로드
    public void InventorySlotInit(List<Item> data, InventoryFilterType filter)
    {
        SlotListInit();
        inventoryFilter = filter;

        foreach (Item item in data)
        {
            switch (inventoryFilter) // 불러와야할 아이템 종류에 따라 아이템 출력
            {
                case InventoryFilterType.Collection:
                    if (item is Collection collection)
                    {
                        GameObject slot = Instantiate(slotPrefab, slotTransform);
                        slot.GetComponent<InventorySlot>().ItemInit(collection);
                        slotList.Add(slot);
                    }
                    break;
                case InventoryFilterType.Potion:
                    if (item is Potion potion)
                    {
                        GameObject slot = Instantiate(slotPrefab, slotTransform);
                        slot.GetComponent<InventorySlot>().ItemInit(potion);
                        slotList.Add(slot);
                    }
                    break;
                case InventoryFilterType.Tool:
                    if (item is Tool tool)
                    {
                        GameObject slot = Instantiate(slotPrefab, slotTransform);
                        slot.GetComponent<InventorySlot>().ItemInit(tool);
                        slotList.Add(slot);
                    }
                    break;
            }
        }
    }

    // 출력중인 인벤토리 슬롯 초기화
    private void SlotListInit()
    {
        foreach (GameObject slot in slotList)
            Destroy(slot);

        slotList.Clear();
    }

    // 아이템 사용 정보 반영
    private void ItemUse(Item item)
    {
        inventoryData.items.Find(x => x == item).count--;
        InventoryUpdate();
    }

    // 인벤토리 정보 갱신
    private void InventoryUpdate()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<InventorySlot>().
                ItemInit(inventoryData.items[i]);
        }
    }

    // 필터 정보에따라 인벤토리 슬롯을 정렬
    private void InentorySorting(int filterType, bool orderType)
    {
        List<Item> tempList = new List<Item>();

        foreach (Item item in inventoryData.items)
            tempList.Add(InventoryItemTypeFilter(item));

        // List 에서 null인 부분 제거
        tempList.RemoveAll(x => x == null);

        switch ((ItemFilterType)filterType)
        {
            case ItemFilterType.Name: // 이름순 정렬
                if(orderType)
                    tempList.Sort((x, y) => x.itemData.itemName.CompareTo(y.itemData.itemName));
                else
                    tempList.Sort((x, y) => y.itemData.itemName.CompareTo(x.itemData.itemName));
                break;
            case ItemFilterType.Capacity: // 소지 갯수순서 정렬
                if (orderType)
                    tempList.Sort((x, y) => x.count.CompareTo(y.count));
                else
                    tempList.Sort((x, y) => y.count.CompareTo(x.count));
                break;
            case ItemFilterType.Rating: // 아이템 등급순 정렬
                if (orderType)
                    tempList.Sort((x, y) => x.itemData.rating.CompareTo(y.itemData.rating));
                else
                    tempList.Sort((x, y) => y.itemData.rating.CompareTo(x.itemData.rating));
                break;
        }

        InventorySlotInit(tempList, inventoryFilter);
    }

    private Item InventoryItemTypeFilter(Item item)
    {
        // 아이템의 유형을 검사해서 자동으로 알맞는 아이템 타입으로 다운 캐스팅
        switch (inventoryFilter)
        {
            case InventoryFilterType.Collection when item is Collection collection:
                return collection;
            case InventoryFilterType.Potion when item is Potion potion:
                return potion;
            case InventoryFilterType.Tool when item is Tool tool:
                return tool;
            default:
                return null;
        }
    }
}
