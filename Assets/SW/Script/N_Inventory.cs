using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEditor.Progress;

[System.Serializable]
public class N_Inventory : MonoBehaviour
{
    [SerializeField] public List<BaseItemData> itemDB = new List<BaseItemData>();
    [SerializeField] public List<Item> items = new List<Item>();

    private void Awake()
    {
        foreach (BaseItemData itemData in itemDB)
        {
            switch (itemData)
            {
                case BasePotionData potion:
                    items.Add(new Potion(potion));
                    break;
                case BaseCollectionData collection:
                    items.Add(new Collection(collection));
                    break;
                case BaseToolData tool:
                    items.Add(new Tool(tool));
                    break;
            }
        }

    }
}