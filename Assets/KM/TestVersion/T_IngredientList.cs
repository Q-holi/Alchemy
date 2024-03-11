using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_IngredientList : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private N_InventoryData inventoryData;

    private void Start()
    {
        inventoryData = gameObject.GetComponent<N_Inventory>().inventoryData;
        InventoryInit(inventoryData);
    }

    public void InventoryInit(N_InventoryData data)
    {
        inventoryData = data;

        foreach (Collection item in inventoryData.collection)
        {
            GameObject slot = Instantiate(slotPrefab, slotTransform);
            slot.GetComponent<T_IngredientSlot>().ItemInit(item);
        }

    }
}
