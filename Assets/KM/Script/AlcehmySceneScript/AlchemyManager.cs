using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Android.Types;

public class AlchemyManager : MonoBehaviour
{
    public static AlchemyManager instance;

    [SerializeField] private InventoryList ingredientList;
    public InventoryList IngredientList { get => ingredientList; }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        List<Collection> ingredientData = ingredientList.Inventory.inventoryData.collections;
        ingredientList.InventoryInit(ingredientData);
    }
}
