using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    [SerializeField] private SO_CharacterData characterData;
    // 유닛들을 불러올 DB
    public static SortedDictionary<string, CharacterData> unitDB =
        new SortedDictionary<string, CharacterData>();

    [SerializeField] private GameObject battleUnitPrefab;       // 화면에 배치될 유닛오브젝트

    [SerializeField] private GameObject[] unitobj = new GameObject[4];        // 실제 화면에 배치된 유닛들
    [SerializeField] private Transform[] unitPosition = new Transform[4];   // 유닛이 배치될 위치

    public bool targeting;      // 선택된 대상이 있는지 검사
    public static float turnTimeScale = 1.0f;      // 턴 관리용 타임스케일 변수

    public string[] battleUnits = new string[4];      // 임시 전투 유닛 생성

    protected override void Awake()
    {
        base.Awake();
        CreateTempChracterData();
    }

    private void Start()
    {
        BattleEventHandler.GetOrder += OnBringOrder;
        BattleEventHandler.ReturnOrder += OnReturnOrder;

        InventoryManager.Instance.InventorySlotInit(InventoryFilterType.Potion);
        SetBattleScene(battleUnits);
    }

    public void SetBattleScene(string[] name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            GameObject unit = Instantiate(battleUnitPrefab, unitPosition[i]);
            unit.GetComponent<BattleUnitStatus>().SetBattleData(name[i]);
            unitobj[i] = unit;
        }
    }

    private void OnBringOrder()
    {
        turnTimeScale = 0.0f;
    }

    private void OnReturnOrder()
    {
        turnTimeScale = 1.0f;
    }

    private void CreateTempChracterData()
    {
        foreach (CharacterData itemDetails in characterData.characterDatas)
        {
            unitDB.Add(itemDetails.defaultStatus.name, itemDetails);
        }
    }
}