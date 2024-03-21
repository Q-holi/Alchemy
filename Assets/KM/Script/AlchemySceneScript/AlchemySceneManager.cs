using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlchemySceneManager : MonoBehaviour
{
    public static AlchemySceneManager instance;

    [SerializeField] private Inventory ingredientList;
    public Inventory IngredientList { get => ingredientList; }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        ingredientList.InventoryInit(ingredientList.InventoryData.items);
    }
}
