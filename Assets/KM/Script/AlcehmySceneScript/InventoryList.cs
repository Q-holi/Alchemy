using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹
    private List<GameObject> slotList = new List<GameObject>();

    public void Start()
    {
        InventoryInit(AlchemyManager.instance.Inventory.inventoryData.collections); // 아이템 정보 설정
    }

    public void InventoryInit(List<Collection> data) // 인벤토리 데이터기반 아이템 설정
    {
        foreach (Collection item in data)
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<InventorySlot>().ItemInit(item);
            slotList.Add(slot);
        }
    }
    public void InventoryInit(List<Potion> data) // 인벤토리 데이터기반 아이템 설정
    {
        foreach (Potion item in data)
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<InventorySlot>().ItemInit(item);
            slotList.Add(slot);
        }
    }

    public void InventoryUpdate(N_InventoryData data)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<InventorySlot>().ItemInit(data.collections[i]);
        }
    }
}
