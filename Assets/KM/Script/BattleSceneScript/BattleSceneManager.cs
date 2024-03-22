using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager instance;

    [SerializeField] private Inventory potionList;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        potionList.InventorySlotInit(potionList.InventoryData.items,
                            InventoryFilterType.Potion);
    }
}
