using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    [SerializeField] private SO_CharacterData characterData;
    // 유닛들을 불러올 DB
    public static SortedDictionary<string, CharacterData> unitDB =
        new SortedDictionary<string, CharacterData>();

    [SerializeField] private GameObject battleUnitPrefab;
    [SerializeField] private InventoryManager potionList;
    public bool targeting;      // 선택된 대상이 있는지 검사
    public static float turnTimeScale = 1.0f;      // 턴 관리용 타임스케일 변수

    protected override void Awake()
    {
        base.Awake();
        CreateTempChracterData();
    }

    private void Start()
    {
        BattleEventHandler.GetTurn += OnSomebodyTurn;

        potionList.InventorySlotInit(InventoryFilterType.Potion);
    }

    private void OnSomebodyTurn()
    {
        turnTimeScale = 0.0f;
    }

    private void CreateTempChracterData()
    {
        foreach (CharacterData itemDetails in characterData.characterDatas)
        {
            unitDB.Add(itemDetails.defaultStatus.name, itemDetails);
        }
    }
}
