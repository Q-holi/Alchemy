using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    // 유닛들을 불러올 DB
    public Dictionary<string, BattleCharacterData> unitDB =
        new Dictionary<string, BattleCharacterData>();

    [SerializeField] private InventoryManager potionList;

    private void Start()
    {
        potionList.InventorySlotInit(InventoryFilterType.Potion);
    }

    private void ItemDBLoad(string filepath = "")
    {
        string path = "Assets/Scriptable Object/Items";
        if (filepath != "") // 파일경로를 입력해줬다면 그 경로를 검사
            path = filepath;

        string[] files = Directory.GetFiles(path);         // 파일 목록
        string[] directories = Directory.GetDirectories(path);        // 하위폴더 목록

        // 폴더내의 파일들 검사
        foreach (string filePath in files)
        {
            // .asset 파일인지 확인
            if (filePath.EndsWith(".asset"))
            {
                // ScriptableObject 데이터 로드
                BattleCharacterData obj = LoadScriptableObject(filePath);
                if (obj != null)
                    unitDB.Add(obj.name, obj);
            }
        }

        // 재귀함수로 하위폴더 아이템 검사
        foreach (string subDirectory in directories)
            ItemDBLoad(subDirectory);
    }

    /// <summary>
    /// 파일이 BaseItemData 형식인지 확인
    /// </summary>
    private BattleCharacterData LoadScriptableObject(string filePath)
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(BattleCharacterData)) as BattleCharacterData;
    }
}
