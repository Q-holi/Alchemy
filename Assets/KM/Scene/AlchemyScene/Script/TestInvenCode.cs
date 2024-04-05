using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testinventory : MonoBehaviour
{
    public static testinventory instance;

    [SerializeField] private Transform slotTransform;
    [SerializeField] public GameObject slotPrefab;
    [SerializeField] public GameObject selectItemPrefab;

    private List<GameObject> slotList = new List<GameObject>();
    public bool isDragging;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        foreach (List<InventoryItem> items in InventoryManager.Instance.inventoryLists)
        {
            foreach (InventoryItem item in items)
            {
                GameObject slot = Instantiate(slotPrefab, slotTransform);
                slot.GetComponent<InventorySlot>().ItemInit(item.itemCode);
                slotList.Add(slot);
            }
        }
    }
}
