using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Collection[] collection;
}

public class InventoryData : MonoBehaviour
{
    [SerializeField] public PlayerData playerData;

    string collectionJsonFilePathtest = "_Data/CollectionSaved_Infotest.json";
    private void Awake()
    {
        LoadJsonData();
    }
    private void OnApplicationQuit()
    {
        SavePlayerDataToJson();
    }
    public void LoadJsonData()
    {
        if (!File.Exists(collectionJsonFilePathtest))
            return;

        string jsonText = File.ReadAllText(collectionJsonFilePathtest);
        playerData = JsonUtility.FromJson<PlayerData>(jsonText);
    }

    [ContextMenu("To Json Data")] // 컴포넌트 메뉴에 아래 함수를 호출하는 To Json Data 라는 명령어가 생성됨
    void SavePlayerDataToJson()
    {
        // ToJson을 사용하면 JSON형태로 포멧팅된 문자열이 생성된다  
        string jsonData = JsonUtility.ToJson(playerData,true);
 
        // 파일 생성 및 저장
        File.WriteAllText(collectionJsonFilePathtest, jsonData);

    }

}
