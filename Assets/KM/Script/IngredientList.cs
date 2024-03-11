using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientList : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹

    public void Start()
    {
        InventoryInit(AlchemyManager.instance.Inventory.inventoryData); // 아이템 정보 설정
    }

    public void InventoryInit(N_InventoryData data) // 인벤토리 데이터기반 아이템 설정
    {
        foreach (Collection item in data.collection)
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<IngredientSlot>().ItemInit(item);
        }
    }
}
