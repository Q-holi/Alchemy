using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SW_InventoryData
{
    [SerializeField] public List<Collection> collection;
    [SerializeField] public List<Collection_Tool> collection_Tool;

    public List<IItem> CombineAndSortByInventoryIndexNumber()
    {
        List<IItem> combinedList = new List<IItem>();
        combinedList.AddRange(collection);
        combinedList.AddRange(collection_Tool);

        // Item 클래스 안에 있는 InventoryIndexNumber 기준으로 인벤토리에 보여줄 Item 리스트 재 정렬
        combinedList = combinedList.OrderBy(item =>
        {
            if (item is Collection)
                return ((Collection)item).InventoryIndexNumber;
            else if (item is Collection_Tool)
                return ((Collection_Tool)item).InventoryIndexNumber;
            else
                throw new ArgumentException("Invalid type in the combined list.");
        }).ToList();

        return combinedList;
    }
}

public class SW_Inventory : MonoBehaviour
{
    [SerializeField] static private SW_InventoryData inventoryData;
    public List<IItem> Items = new List<IItem>();//-- 추후 인벤토리를 보여줄시 리스트 출력 

    string sw_JsonFilePathtest = "_Data/SW_Inventory_NoIncoding.json";

    private void Awake()
    {
        LoadJsonData();
        Items = inventoryData.CombineAndSortByInventoryIndexNumber();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (IItem item in Items)
            {
                Debug.Log(item.KeyValue);
            }
        }
            
    }
    private void OnApplicationQuit()
    {
        //SavePlayerDataToJson();
    }
    public void LoadJsonData()
    {
        //--지정된 경로에 파일이 없다면 Load를 하지 않고 넘어간다.
        if (!File.Exists(sw_JsonFilePathtest))
            return;

        string jsonText = File.ReadAllText(sw_JsonFilePathtest);
        SW_InventoryData itemData = JsonConvert.DeserializeObject<SW_InventoryData>(jsonText);
        //byte[] bytes = System.Convert.FromBase64String(jsonText);
        //string reformat = System.Text.Encoding.UTF8.GetString(bytes);
        inventoryData = JsonUtility.FromJson<SW_InventoryData>(jsonText);
    }

    [ContextMenu("To Json Data")] // 컴포넌트 메뉴에 아래 함수를 호출하는 To Json Data 라는 명령어가 생성됨
    void SavePlayerDataToJson()
    {
        // ToJson을 사용하면 JSON형태로 포멧팅된 문자열이 생성된다  
        string jsonData = JsonUtility.ToJson(inventoryData, true);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        ////--ToBase64String 인코딩 하여 저장
        //string format = System.Convert.ToBase64String(bytes);

        // 파일 생성 및 저장
        File.WriteAllText(sw_JsonFilePathtest, jsonData);
    }
}
