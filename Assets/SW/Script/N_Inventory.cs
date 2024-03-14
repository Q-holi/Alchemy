using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public class N_InventoryData
{
    public List<Collection> collections;
    public List<Potion> potions;
}

public class N_Inventory : MonoBehaviour
{
    public bool incoding64;

    public N_InventoryData inventoryData;

    string sw_JsonFilePathtest = "_Data/New_Inventory.json";

    private bool AddInventoryData(IItem item) =>
        inventoryData.collections.Any(x => x.InventoryIndexNumber == item.InventoryIndexNumber);

    private void Awake()
    {
        inventoryData = new N_InventoryData();
        inventoryData.collections = new List<Collection>();
        inventoryData.potions = new List<Potion>();
        LoadJsonData();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //// 예시 아이템 생성
            //IItem item = new N_Collection();
            //
            //// CreateCollection 메서드 호출
            //N_Collection collection = ItemFactory.CreateCollection( 5, 10, 11);

            // 예시 아이템 생성
            Potion item = new NormalPotion();
            inventoryData.potions.Add(item);
        }

    }
    private void OnApplicationQuit()
    {
        SavePlayerDataToJson();
    }
    public void LoadJsonData()
    {
        //--지정된 경로에 파일이 없다면 Load를 하지 않고 넘어간다.
        if (!File.Exists(sw_JsonFilePathtest))
            return;


        string jsonText = File.ReadAllText(sw_JsonFilePathtest);
        try
        {
            //--Json이 로드 될때 base64로 인코딩이 되어 있으면 디코딩 후 읽어온다.
            byte[] decodedBytes = Convert.FromBase64String(jsonText);
            string reformat = System.Text.Encoding.UTF8.GetString(decodedBytes);
            inventoryData = JsonUtility.FromJson<N_InventoryData>(reformat);

            if (inventoryData.potions == null)
                inventoryData.potions = new List<Potion>();

            if (inventoryData.collections == null)
                inventoryData.collections = new List<Collection>();

        }
        catch (FormatException)
        {
            inventoryData = JsonUtility.FromJson<N_InventoryData>(jsonText);
            if (inventoryData.potions == null)
                inventoryData.potions = new List<Potion>();

            if (inventoryData.collections == null)
                inventoryData.collections = new List<Collection>();
        }
    }

    [ContextMenu("To Json Data")] // 컴포넌트 메뉴에 아래 함수를 호출하는 To Json Data 라는 명령어가 생성됨
    void SavePlayerDataToJson()
    {

        string jsonData = JsonUtility.ToJson(inventoryData, true);
        if (incoding64)//--base64로 인코딩 체크 여부 확인
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            //--ToBase64String 인코딩 하여 저장
            string format = System.Convert.ToBase64String(bytes);
            // 파일 생성 및 저장
            File.WriteAllText(sw_JsonFilePathtest, format);
        }
        else
        {
            // 파일 생성 및 저장
            File.WriteAllText(sw_JsonFilePathtest, jsonData);
        }

    }

}