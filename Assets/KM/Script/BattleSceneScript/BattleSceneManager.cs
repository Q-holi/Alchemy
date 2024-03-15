using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager instance;

    [SerializeField] private InventoryList ingredientList;
    public InventoryList IngredientList { get => ingredientList; }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        //List<Potion> ingredientData = ingredientList.Inventory.inventoryData.potions;
        //ingredientList.InventoryInit(ingredientData);
    }
}
