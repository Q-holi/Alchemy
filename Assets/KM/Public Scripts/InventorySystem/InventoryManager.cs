using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(GenerateGUID))]
//-- 클래스가 GenerateGUID 컴포넌트를 필요로 한다는 것을 나타냅니다.
public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private SO_ItemList itemList = null;   // 아이템 DB 데이터 받아오기
    private SortedDictionary<int, ItemDetails> itemDetailsDictionary;     // 아이템 DB 데이터 접근 가능한 객체

    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹

    public List<InventoryItem> inventoryLists = new List<InventoryItem>();  // 인벤토리의 아이템들
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    private InventoryFilterType inventoryFilter;    // 인벤토리 아이템 유형 필터
    public bool isDragging = false;     // 아이템 드래그 감지

    public GameObject SelectItemPrefab { get => selectItemPrefab; }

    private string _iSaveableUniqueID; //--<GenerateGUID>().GUID값을 받는다.
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave; // 씬에 저장된 아이템 리스트를 들고있는 객체
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    protected override void Awake()
    {
        base.Awake();
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
        CreateItemDetailsDictionary();
    }

    #region Event Setting
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
    #endregion

    /// <summary>
    /// 인벤토리 데이터로 인벤토리 정보 로드
    /// externData 에 다른 List 를 넘겨주면, 그 List로 인벤토리 초기화
    /// </summary>
    public void InventorySlotInit(InventoryFilterType filter, List<InventoryItem> externData = null)
    {
        List<InventoryItem> initList = inventoryLists;
        if (externData != null)
            initList = externData;

        SlotListInit();
        inventoryFilter = filter;

        foreach (InventoryItem item in initList)
        {
            ItemDetails targetItem = GetItemDetails(item.itemCode);
            switch (filter)
            {
                case InventoryFilterType.Collection:
                    if (GetItemDetails(item.itemCode).collection)
                        MakeSlot(targetItem);
                    break;
                case InventoryFilterType.Potion:
                    if (GetItemDetails(item.itemCode).potion)
                        MakeSlot(targetItem);
                    break;
                case InventoryFilterType.Tool:
                    if (GetItemDetails(item.itemCode).tool)
                        MakeSlot(targetItem);
                    break;
            }
        }
    }

    /// <summary>
    /// 인벤토리 슬롯 생성하기
    /// </summary>
    private void MakeSlot(ItemDetails itemDetail)
    {
        if (itemDetail != null)   // 왜인지 모르겠는데 null인 객체가 하나 생성됨. 나중에 알아보기
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<InventorySlot>().ItemInit(itemDetail);
            slotList.Add(slot);
        }
        return;
    }

    /// <summary>
    /// 출력중인 인벤토리 슬롯 초기화
    /// </summary>
    private void SlotListInit()
    {
        foreach (GameObject slot in slotList)
            Destroy(slot);

        slotList.Clear();
    }

    /// <summary>
    /// 아이템 사용시 인벤토리 데이터 업데이트   
    /// </summary>
    private void ItemUse(int keyCode, bool isUse)
    {
        int index = SearchItem(keyCode);
        InventoryItem targetItem;
        if (index != -1)
            targetItem = inventoryLists[index];
        else
        { 
            Debug.Log("Wrong ItemCode");
            return;
        }

        if (isUse) // 아이템 사용시
        {
            targetItem.itemQuantity--;
            inventoryLists[index] = targetItem;

            // 아이템 키에 해당하는 아이템 찾아서, 그 슬롯의 정보 업데이트
            foreach (GameObject slot in slotList)
            {
                if (keyCode == slot.GetComponent<InventorySlot>().GetItem)
                {
                    slot.GetComponent<InventorySlot>().
                        ItemInit(GetItemDetails(keyCode));
                    break;
                }
            }
            Debug.Log("Item {" + itemDetailsDictionary[targetItem.itemCode].name + "} Use Success");
        }
        else // 아이템 사용 취소 or 실패 시
        {
            targetItem.itemQuantity++;
            inventoryLists[index] = targetItem;

            foreach (GameObject slot in slotList)
            {
                if (keyCode == slot.GetComponent<InventorySlot>().GetItem)
                {
                    slot.GetComponent<InventorySlot>().
                        ItemInit(GetItemDetails(keyCode));
                    break;
                }
            }
            Debug.Log("Item Use Cancel or Failed");
        }
    }

    public int SearchItem(int keyCode)
    {
        InventoryItem findItem = inventoryLists.Find(x => x.itemCode == keyCode);
        if (!findItem.Equals(default(InventoryItem)))
            return inventoryLists.IndexOf(findItem);

        return -1;
    }

    /// <summary>
    /// 필터 정보에따라 인벤토리 슬롯을 정렬
    /// </summary>
    private void InentorySorting(int filterType, bool orderType)
    {
        switch ((ItemFilterType)filterType)
        {
            case ItemFilterType.Name: // 이름순 정렬
                if (orderType)
                    inventoryLists.Sort((x, y) => itemDetailsDictionary[x.itemCode].name.
                                                    CompareTo(itemDetailsDictionary[y.itemCode].name));
                else
                    inventoryLists.Sort((x, y) => itemDetailsDictionary[y.itemCode].name.
                                                    CompareTo(itemDetailsDictionary[x.itemCode].name));
                break;
            case ItemFilterType.Capacity: // 소지 갯수량 기준 정렬
                if (orderType)
                    inventoryLists.Sort((x, y) => x.itemQuantity.CompareTo(y.itemQuantity));
                else
                    inventoryLists.Sort((x, y) => y.itemQuantity.CompareTo(x.itemQuantity));
                break;
            case ItemFilterType.Rating: // 아이템 등급 기준 정렬
                if (orderType)
                    inventoryLists.Sort((x, y) => itemDetailsDictionary[x.itemCode].itemRating.
                                                    CompareTo(itemDetailsDictionary[y.itemCode].itemRating));
                else
                    inventoryLists.Sort((x, y) => itemDetailsDictionary[y.itemCode].itemRating.
                                                    CompareTo(itemDetailsDictionary[x.itemCode].itemRating));
                break;
        }
    
        InventorySlotInit(inventoryFilter, inventoryLists);
    }

    #region SW_SaveLoad
    public void ISaveableStoreScene(string sceneName)
    {
        //기존에 있던 같은 이름의 scene을 지웁니다.-> Scene을 새롭게 업데이트 하는 방식
        GameObjectSave.itemData.Remove(sceneName);

        //-- 이후 현제 Scene의 있는 아이템 전부를 가져와 SceneItem형식으로 List에 저장한다. 
        List<InventoryItem> itemList = inventoryLists;
        //Item[] itemsInScene = FindObjectsOfType<Item>();

        ItemSave itemSave = new ItemSave();
        itemSave.listItemDictionary = new Dictionary<string, List<InventoryItem>>();
        itemSave.listItemDictionary.Add("ItemList", itemList);

        GameObjectSave.itemData.Add(sceneName, itemSave);
    }

    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new SortedDictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            InventoryItem tempItem = new InventoryItem();
            tempItem.itemCode = itemDetails.itemCode;
            tempItem.itemQuantity = 10;
            inventoryLists.Add(tempItem);
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
            return itemDetails;
        else
            return null;
    }
    public void LoadItemSaveData(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
        }

        if (GameObjectSave.itemData.TryGetValue("TestSaveScene", out ItemSave itemSave))//--sceneSave ->  Dictionary<string, List<SceneItem>> SceneItem 리스트를 가져온다.
        {
            if (itemSave.listItemDictionary != null && itemSave.listItemDictionary.TryGetValue("ItemList", out List<InventoryItem> itemList))
            {
                LoadInventoryDataSetSlot(itemList);
            }
        }
    }
    private void LoadInventoryDataSetSlot(List<InventoryItem> itemList)
    {
        foreach (InventoryItem item in itemList)
        {
            MakeSlot(GetItemDetails(item.itemCode));
        }
    }
    #endregion
}