using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] public List<Collection> collections = new List<Collection>();
    private int maxInventoryCount = 32;

    string collectionJsonFilePath = "_Data/CollectionSaved_Info.json";
    string collectionJsonFilePathtest = "_Data/CollectionSaved_Infotest.json";

    private void Awake()
    {
       
    }
    private void OnEnable()
    {

    }
}
