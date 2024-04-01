using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class InventoryManager_SW : Singleton<InventoryManager_SW>
{
<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
    public static Dictionary<int, BaseItemData> itemDB = new Dictionary<int, BaseItemData>();         // 아이템 DB
========
    Collection,
    Potion,
    Tool
}

[RequireComponent(typeof(GenerateGUID))]
//-- 클래스가 GenerateGUID 컴포넌트를 필요로 한다는 것을 나타냅니다.
public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private SO_ItemList itemList = null;   // 아이템 DB 데이터 받아오기
    private SortedDictionary<int, ItemDetails> itemDetailsDictionary;     // 아이템 DB 데이터 접근 가능한 객체
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs

    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹

<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
    private List<Item> items = new List<Item>();            // 실제 인벤토리의 아이템들
========
    public List<InventoryItem> inventoryLists = new List<InventoryItem>();  // 인벤토리의 아이템들
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    private InventoryFilterType_SW inventoryFilter;    // 인벤토리 아이템 유형 필터
    public bool isDragging = false;     // 아이템 드래그 감지

    public GameObject SelectItemPrefab { get => selectItemPrefab; }

<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
    protected override void Awake()
    {
        base.Awake();
        ItemDBLoad();
        TempItemGenerater();
========
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
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
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

    #region ItemDB Load
    /// <summary>
    /// Scriptable Object 불러와서 itemDB 생성
    /// </summary>
    private void ItemDBLoad(string filepath = "")
    {
        string path = "Assets/Scriptable Object/Items";
        if (filepath != "") // 파일경로를 입력해줬다면 그 경로를 검사
            path = filepath;

        string[] files = Directory.GetFiles(path);         // 파일 목록
        string[] directories = Directory.GetDirectories(path);        // 하위폴더 목록

        // 폴더내의 파일들 검사
        foreach (string filePath in files)
        {
            // .asset 파일인지 확인
            if (filePath.EndsWith(".asset"))
            {
                // ScriptableObject 데이터 로드
                BaseItemData obj = LoadScriptableObject(filePath);
                if (obj != null)
                    itemDB.Add(obj.keyCode, obj);
            }
        }

        // 재귀함수로 하위폴더 아이템 검사
        foreach (string subDirectory in directories)
            ItemDBLoad(subDirectory);
    }

    /// <summary>
    /// 파일이 BaseItemData 형식인지 확인
    /// </summary>
    private BaseItemData LoadScriptableObject(string filePath)
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(BaseItemData)) as BaseItemData;
    }
    #endregion

    /// <summary>
    /// 인벤토리 데이터로 인벤토리 정보 로드
    /// externData 에 다른 List 를 넘겨주면, 그 List로 인벤토리 초기화
    /// </summary>
    public void InventorySlotInit(InventoryFilterType_SW filter, List<Item> externData = null)
    {
        List<Item> initList = items;
        if (externData != null)
            initList = externData;

        SlotListInit();
        inventoryFilter = filter;

        foreach (Item item in initList)
        {
            MakeSlot(UtilFunction.InventoryItemTypeFilter(item, inventoryFilter));
        }
    }

    /// <summary>
<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
========
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
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
    /// 출력중인 인벤토리 슬롯 초기화
    /// </summary>
    private void SlotListInit()
    {
        foreach (GameObject slot in slotList)
            Destroy(slot);

        slotList.Clear();
    }

    /// <summary>
    /// 인벤토리 슬롯 생성하기
    /// </summary>
    private void MakeSlot(Item item)
    {
        if (item != null)   // 왜인지 모르겠는데 null 인 Item 이 하나 생성됨. 나중에 알아보기
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<InventorySlot>().ItemInit(item);
            slotList.Add(slot);
        }
        return;
    }

    /// <summary>
    /// 아이템 사용시 인벤토리 데이터 업데이트
    /// </summary>
    private void ItemUse(int itemKey, bool isUse)
    {
        if (isUse) // 아이템 사용시
        {
<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
            items.Find(x => x.itemkey == keyCode).count--;
========
            InventoryItem findItem = inventoryLists.Find(x => x.itemCode == itemKey);
            if (!findItem.Equals(default(InventoryItem)))
                findItem.itemQuantity--;

>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
            // 아이템 키에 해당하는 아이템 찾아서, 그 슬롯의 정보 업데이트
            foreach (GameObject slot in slotList)
            {
                if (itemKey == slot.GetComponent<InventorySlot>().GetItem)
                {
                    slot.GetComponent<InventorySlot>().
<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
                        ItemInit(items.Find(x => x.itemkey == keyCode));
========
                        ItemInit(GetItemDetails(itemKey));
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
                    break;
                }
            }
            Debug.Log("Item Use Success");
        }
        else // 아이템 사용 취소 or 실패 시
        {
            InventoryItem findItem = inventoryLists.Find(x => x.itemCode == itemKey);
            if (!findItem.Equals(default(InventoryItem)))
                findItem.itemQuantity++;

            foreach (GameObject slot in slotList)
            {
                if (itemKey == slot.GetComponent<InventorySlot>().GetItem)
                {
                    slot.GetComponent<InventorySlot>().
                        ItemInit(GetItemDetails(itemKey));
                    break;
                }
            }
            Debug.Log("Item Use Cancel");
        }
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

<<<<<<<< HEAD:Assets/DevelopmentProgress/Inventory/InventoryManager_SW.cs
    /// <summary>
    /// 임시 데이터 생성용, 나중에 지울것
    /// </summary>
    private void TempItemGenerater()
    {
        foreach (KeyValuePair<int, BaseItemData> data in itemDB)
        {
            if (data.Key >= 1000 && data.Key < 2000)
                items.Add(new Collection(data.Key));
            else if (data.Key >= 2000 && data.Key < 3000)
                items.Add(new Potion(data.Key));
            else if (data.Key >= 3000 && data.Key < 4000)
                items.Add(new Tool(data.Key));
        }
    }
========
    #endregion

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
>>>>>>>> d14931a (Merge branch 'KM_Branch' of https://github.com/Q-holi/Alchemy into KM_Branch):Assets/KM/Public Scripts/InventorySystem/InventoryManager.cs
}
