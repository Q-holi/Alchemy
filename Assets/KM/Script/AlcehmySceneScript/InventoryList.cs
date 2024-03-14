using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;           // 슬롯 출력 위치
    [SerializeField] private GameObject slotPrefab;             // 재료 슬롯 프리팹
    [SerializeField] private N_Inventory inventory;             // 인벤토리 데이터
    private List<GameObject> slotList = new List<GameObject>(); // 현재 인벤토리에 생성된 아이템 슬롯들

    [SerializeField] private IItem selectItem;        // 선택된 아이템
    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트

    public bool isDragging = false;     // 아이템 드래그 감지

    #region GetSet
    public GameObject SelectItemPrefab { get => selectItemPrefab; }
    public IItem SelectItem { get => selectItem; set => selectItem = value; }
    #endregion

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

    public void ItemUse(bool isUse, Collection item)
    {
        if (isUse)
        {
            //PotionMarker marker = potionMap.GetPotionMarker.GetComponent<PotionMarker>();

            inventory.inventoryData.collections.Find(x => x == item).count--;
            InventoryUpdate(inventory.inventoryData);

            //Vector3 startPos = marker.gameObject.transform.localPosition;   // 시작점
            //Vector3 endPos = startPos +
            //    (new Vector3(item.Green_Option - item.Alpha_Option, item.Red_Option - item.Blue_Option, 0.0f)) / 10.0f; // 목적지
            //StartCoroutine(marker.MovePotionCorutine(startPos, endPos));    // 포션 위치 움직이는 코루틴 실행

            //previewLine.gameObject.SetActive(false);
        }
        else
        {
            inventory.inventoryData.collections.Find(x => x == item).count++;
            InventoryUpdate(inventory.inventoryData);
        }
    }
}
