using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.Progress;

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
        if (File.Exists(collectionJsonFilePath))
        {
            string jsonText = File.ReadAllText(collectionJsonFilePath);
            // JSON 문자열을 Deserialize하여 List에 추가
            //collections.AddRange(JsonConvert.DeserializeObject<List<Collection>>(jsonText));
            items.AddRange(JsonConvert.DeserializeObject<List<Item>>(jsonText));
        }

        Collection data1 = new Collection();
        data1.keyvalue = 1001;
        data1.name = "Test";
        data1.rating = Rating.Normal;
        data1.texture2DImagePath = "";
        data1.count = 1001;
        data1.red_Option = 6;
        data1.black_Option = 8;

        collections.Add(data1);
        Debug.Log(collections[0].name);
        string json = JsonUtility.ToJson(collections);
        File.WriteAllText(collectionJsonFilePathtest, json);
    }
    private void OnApplicationQuit()
    {

        //SaveCollectionsToJson();
    }

    void SaveInventory()
    {

    }
    public void SaveCollectionsToJson()
    {
        List<string> serializedCollections = new List<string>();
        foreach (Collection collection in collections)
        {
            serializedCollections.Add(collection.Serialize());
        }
        string json = JsonUtility.ToJson(serializedCollections);
        File.WriteAllText(collectionJsonFilePathtest, json);
        // 저장 로직
    }
}
