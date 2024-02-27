using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    private int maxInventoryCount = 32;

    private void OnEnable()
    {
        string jsonFilePath = "_Data/Saved_Item_Info.json";

        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);

            // JSON 문자열을 Deserialize하여 List에 추가
            items.AddRange(JsonConvert.DeserializeObject<List<Item>>(jsonText));
        }
    }
}
