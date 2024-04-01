using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlchemySceneManager : MonoBehaviour
{
    [SerializeField] private InventoryManager ingredientList;

    private void OnEnable()
    {
        ingredientList.InventorySlotInit(InventoryFilterType.Collection);
    }
}
