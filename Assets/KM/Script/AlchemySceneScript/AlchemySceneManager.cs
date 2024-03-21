using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Android.Types;

public class AlchemySceneManager : MonoBehaviour
{
    public static AlchemySceneManager instance;

    [SerializeField] private InventoryList ingredientList;
    public InventoryList IngredientList { get => ingredientList; }

    private void Start()
    {
        if (instance == null)
            instance = this;

        ingredientList.InventoryInit(ingredientList.Inventory.items);
    }
}
