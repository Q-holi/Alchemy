using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlchemySceneManager : MonoBehaviour
{
    [SerializeField] private Inventory ingredientList;
    public Inventory IngredientList { get => ingredientList; }

    private void Awake()
    {
        ingredientList.InventorySlotInit(ingredientList.InventoryData.items,
                                    InventoryFilterType.Collection);
    }
}
