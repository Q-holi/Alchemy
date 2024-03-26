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
    public static Dictionary<int, BaseItemData> itemDB = new Dictionary<int, BaseItemData>();         // 아이템 DB

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
    public void InventorySlotInit(InventoryFilterType filter, List<Item> externData = null)
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
    private void ItemUse(int keyCode)
    {
        items.Find(x => x.itemkey == keyCode).count--;
        InventorySlotInit(inventoryFilter, items);
    }

    /// <summary>
    /// 필터 정보에따라 인벤토리 슬롯을 정렬
    /// </summary>
    private void InentorySorting(int filterType, bool orderType)
    {
        // 임시 아이템 리스트
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
                    tempList.Sort((x, y) => itemDB[x.itemkey].itemName.CompareTo(itemDB[y.itemkey].itemName));
                else
                    tempList.Sort((x, y) => itemDB[y.itemkey].itemName.CompareTo(itemDB[x.itemkey].itemName));
                break;
            case ItemFilterType.Capacity: // 소지 갯수순서 정렬
                if (orderType)
                    tempList.Sort((x, y) => x.count.CompareTo(y.count));
                else
                    tempList.Sort((x, y) => y.count.CompareTo(x.count));
                break;
            case ItemFilterType.Rating: // 아이템 등급순 정렬
                if (orderType)
                    tempList.Sort((x, y) => itemDB[x.itemkey].rating.CompareTo(itemDB[y.itemkey].rating));
                else
                    tempList.Sort((x, y) => itemDB[y.itemkey].rating.CompareTo(itemDB[x.itemkey].rating));
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
            if (data.Key >= 1000 && data.Key < 2000)
                items.Add(new Collection(data.Key));
            else if (data.Key >= 2000 && data.Key < 3000)
                items.Add(new Potion(data.Key));
            else if (data.Key >= 3000 && data.Key < 4000)
                items.Add(new Tool(data.Key));
        }
    }
}
