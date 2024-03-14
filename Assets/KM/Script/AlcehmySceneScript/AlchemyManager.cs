using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Android.Types;

public class AlchemyManager : MonoBehaviour
{
    [SerializeField] private InventoryList ingredientList;

    private void Awake()
    {
        List<Collection> ingredientData = ingredientList.Inventory.inventoryData.collections;
        ingredientList.InventoryInit(ingredientData);
    }
}
