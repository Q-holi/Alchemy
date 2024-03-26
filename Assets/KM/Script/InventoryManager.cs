using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum InventoryFilterType
{
    Collection,
    Potion,
    Tool
}

public class InventoryManager : Singleton<InventoryManager>
{
    public Dictionary<int, BaseItemData> itemDB = new Dictionary<int, BaseItemData>();         // 아이템 DB

    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹

    private List<Item> items = new List<Item>();            // 실제 인벤토리의 아이템들
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    private InventoryFilterType inventoryFilter;    // 인벤토리 아이템 유형 필터
    public bool isDragging = false;     // 아이템 드래그 감지

    public GameObject SelectItemPrefab { get => selectItemPrefab; }

    protected override void Awake()
    {
        base.Awake();
        ItemDBLoad();
        TempItemGenerater();
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
    /// itemDB 기반 데이터 로드
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
    /// </summary>
    public void InventorySlotInit(InventoryFilterType filter, List<Item> externData = null)
    {
        List<Item> initList = items;
        if (externData != null)
            initList = externData;

        SlotListInit();
        inventoryFilter = filter;

        foreach (Item item in initList)
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
    private void ItemUse(Item item)
    {
        items.Find(x => x == item).count--;
        InventoryUpdate();
    }

    /// <summary>
    /// 인벤토리 정보 갱신
    /// </summary>
    private void InventoryUpdate()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<InventorySlot>().
                ItemInit(items[i]);
        }
    }

    /// <summary>
    /// 필터 정보에따라 인벤토리 슬롯을 정렬
    /// </summary>
    private void InentorySorting(int filterType, bool orderType)
    {
        // 임시 인벤토리 리스트
        List<Item> tempList = new List<Item>();

        // 유형에 맞는 아이템 정보만 필터링
        foreach (Item item in items)
            tempList.Add(UtilFunction.InventoryItemTypeFilter(item, inventoryFilter));

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

        InventorySlotInit(inventoryFilter, tempList);
    }

    /// <summary>
    /// 임시 데이터 생성용, 나중에 지울것
    /// </summary>
    private void TempItemGenerater()
    {
        foreach (KeyValuePair<int, BaseItemData> data in itemDB)
        {
            if (data.Value is BaseCollectionData collectionData)
                items.Add(new Collection(collectionData));
            else if (data.Value is BasePotionData potionData)
                items.Add(new Potion(potionData));
            else if (data.Value is BaseToolData toolData)
                items.Add(new Tool(toolData));
        }
    }
}
