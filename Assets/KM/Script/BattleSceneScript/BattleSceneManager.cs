using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager instance;

    [SerializeField] private InventoryManager potionList;

    private void Start()
    {
        if (instance == null)
            instance = this;

        potionList.InventorySlotInit(InventoryFilterType.Potion);
    }
}
