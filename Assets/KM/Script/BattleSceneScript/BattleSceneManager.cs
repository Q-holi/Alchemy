using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    // 유닛들을 불러올 DB
    public static SortedDictionary<string, BattleCharacterData> unitDB =
        new SortedDictionary<string, BattleCharacterData>();

    [SerializeField] private GameObject battleUnitPrefab;
    [SerializeField] private InventoryManager potionList;
    public bool targeting;      // 선택된 대상이 있는지 검사
    public static float turnTimeScale = 1.0f;      // 턴 관리용 타임스케일 변수

    private void Start()
    {
        BattleEventHandler.GetTurn += OnSomebodyTurn;

        potionList.InventorySlotInit(InventoryFilterType.Potion);
    }

    #region UnitDB Load
    /// <summary>
    /// 전투에 사용될 유닛 데이터 불러오기
    /// </summary>
    private void UnitDBLoad(string filepath = "")
    {
        string path = "Assets/Scriptable Object/BattleData";
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
            UnitDBLoad(subDirectory);
    }

    /// <summary>
    /// 파일이 BattleCharacterData 형식인지 확인
    /// </summary>
    private BattleCharacterData LoadScriptableObject(string filePath)
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(BattleCharacterData)) as BattleCharacterData;
    }
    #endregion

    private void OnSomebodyTurn()
    {
        turnTimeScale = 0.0f;
    }
}
