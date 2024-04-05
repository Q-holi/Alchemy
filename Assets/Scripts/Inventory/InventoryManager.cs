using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventoryManager : Singleton<InventoryManager>
{
    #region 변수
    //--Assets/Scriptable Object Assets/Item/so_ItemList.asset 에 있는 스크립터블 오브젝트
    [SerializeField] private SO_ItemList itemList = null;
    private Dictionary<int, ItemDetails> itemDetailsDictionary;
    [HideInInspector] public int[] inventoryListCapacityIntArray;

    private int[] selectInventoryItem;
    public List<InventoryItem>[] inventoryLists;
    //--▲▲▲실제 아이템이 저장될 인벤토리 인벤토리는[플레이어] [가방]으로 생성이 된다. 
    #endregion

    #region Unity CallBack
    protected override void Awake()
    {
        base.Awake();
        CreateInventoryLists();
        //--아이템 딕셔너리 생성
        CreateItemDetailsDictionary();

        selectInventoryItem = new int[(int)InventoryLocation.count];
        for (int i = 0; i < selectInventoryItem.Length; i++)
            selectInventoryItem[i] = -1;
    }
    #endregion
    /// <summary>
    /// inventoryLists는 inventoryLists[player] 와 inventoryLists[chest]로 생성이 진행된다.
    /// inventoryListCapacityIntArray[player] 와 inventoryListCapacityIntArray[chest]로 생성
    ///  inventoryListCapacityIntArray[player] =  Settings.playerInventoryCapacity; -> 24
    /// </summary>

    #region Custom Methods
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
            inventoryLists[i] = new List<InventoryItem>();


        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];

        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInventoryCapacity;
    }
    /// <summary>
    /// 스크립터블 오브젝트내용을 가져와 itemDetailsDictionary에 itemcode,itemDetails 로 저장한다. 
    /// 사전같은 의미로 itemDetailsDictionary 자체가 사전이며 itemcode 가 목차 개념 목차로 인한 아이템 정보 사전을 검색하여 정보를 가져온다.
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
    }

    /// <summary>
    /// 플레이어의 PickUP 기능에서 가져온 GameObject gameObjectToDelete는 월드에서 아이템을 파괴시키기 위해 가져온것이다.
    /// Item item은 자신의 itemcode만 가지고 있다.  class Item -> [SerializeField] private int _itemCode;
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="item"></param>
    /// <param name="gameObjectToDelete"></param>
    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);
        Destroy(gameObjectToDelete);
    }
    /// <summary>
    ///  List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];이 코드는 inventoryLocation가 player 또는 chest면
    ///  inventoryList에는 inventoryLists[player]에 대한 내용이 전달된다. 
    ///  FindItemInInventory은 해당하는 inventoryLists의 고유얀 itemCode가 중복되는지 검사하고 중복되면 inventoryLists의 인덱스 위치를 반환해준다. Int
    ///  itemPosition = inventoryLists[inventoryLocation]의 동인한 인덱스 위치 반환이 -1이면 inventoryLists에 동일한 itemCode가 없다.
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        else
            AddItemAtPosition(inventoryList, itemCode);

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);

    }
    /// <summary>
    /// 해당하는 인벤토리에 아이템이 없을 시 해당 인벤토리에 itemCode와 수량 =1을 List.add 를 진행한다. 
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);
    }
    /// <summary>
    /// 해당하는 인벤토리에 동인한 itemCode가 있는 경우 해당 인덱스 position에 수량을 itemQuantity+1 시킨다.
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="position"></param>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;

        Debug.ClearDeveloperConsole();
    }

    /// <summary>
    /// 해당하는 inventoryLists에 접근하여 고유한 itemCode가 있는 검사한다. 
    /// 만약 고유한 itemCode가 동일한게 있다면 ex)inventoryLists[player]에 몇번째 인덱스에 있는 인덱스 번호를 반환해준다.
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
            if (inventoryList[i].itemCode == itemCode)
                return i;

        return -1;
    }
    /// <summary>
    /// TryGetValue(itemCode, out itemDetails)) -> 딕셔너리중 itemcode에 해당하는 값을 가져와 itemDetails로 가져온다. 
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
            return itemDetails;
        else
            return null;
    }

    /// <summary>
    /// Returns the itemDetails (from the SO_ItemList) for the currently selected item in the inventoryLocation , or null if an item isn't selected
    ///</summary>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation inventoryLocation)
    {
        int itemCode = GetSelectedInventoryItem(inventoryLocation);

        if (itemCode == -1)
            return null;
        else
            return GetItemDetails(itemCode);
    }
        
    /// <summary>
    /// Get the selected item for inventoryLocation - returns itemCode or -1 if nothing is selected
    /// </summary>
    private int GetSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
       return selectInventoryItem[(int)inventoryLocation];
    }

    /// <summary>
    /// AddItem과 동인하게 해당 인벤토리 리스트를 가져와 고유한 itemCode가 중복이 있는지 먼저 확인한다. 확인 후 중복된 itemcode가 있다면 
    /// 해당 인벤토리 리스트 인덱스의 접근하여 수량을 가져온다.itemQuantity 해당 수량의 -1을 한다. 
    /// 추가적으로 -1한 itemQuantity이 > 0 보다 크면 해당 인벤토리리스트의 인덱스의 수량을 업데이트 한다. 
    /// -1한 itemQuantity이 > 0보다 작으면 또는 0 이면 해당 인벤토리 시스트에 해당 인덱스를 List.RemoveAt()을 진행
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="itemCode"></param>
    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            InventoryItem inventoryItem = new InventoryItem();
            int quantity = inventoryList[itemPosition].itemQuantity - 1;

            if (quantity > 0)
            {
                inventoryItem.itemQuantity = quantity;
                inventoryItem.itemCode = itemCode;
                inventoryList[itemPosition] = inventoryItem;
            }
            else
                inventoryList.RemoveAt(itemPosition);

        }
        // Send event that inventory has been updated
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }
    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;

            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;

            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;

            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;

            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;

            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;

            default:
                itemTypeDescription = itemType.ToString();
                break;
        }
        return itemTypeDescription;
    }
    public void SwapInventoryItems(InventoryLocation inventoryLocation, int slotIndex, int toslotIndex)
    {

        // if fromItem index and toItemIndex are within the bounds of the list, not the same, and greater than or equal to zero
        //if (slotIndex < inventoryLists[(int)inventoryLocation].Count && toslotIndex < inventoryLists[(int)inventoryLocation].Count
        //&& slotIndex != toslotIndex && slotIndex >= 0 && toslotIndex >= 0)
        //{
        //
        //}
        InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][slotIndex];
        InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toslotIndex];

        inventoryLists[(int)inventoryLocation][toslotIndex] = fromInventoryItem;
        inventoryLists[(int)inventoryLocation][slotIndex] = toInventoryItem;

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }
    /// <summary>
    /// selectInventoryItem[(int)inventoryLocation]->[player],[chest]가 생성되어 있는 상태
    /// selectInventoryItem[(int)inventoryLocation] 의 값을 -1로 초기화 시킨다.[현재 선택 되어 있는 아이템이 없다]
    /// </summary>
    /// <param name="inventoryLocation"></param>
    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectInventoryItem[(int)inventoryLocation] = -1;
    }

    /// <summary>
    /// selectInventoryItem[(int)inventoryLocation]->[player],[chest]가 생성되어 있는 상태
    /// selectInventoryItem[(int)inventoryLocation] = itemCode; 현재 고유한 itemCode의 아이템이 선택이 되어는걸 저장한다. 
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="itemCode"></param>
    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, int itemCode)
    {
        selectInventoryItem[(int)inventoryLocation] = itemCode;
    }

}
#endregion