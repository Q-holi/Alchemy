using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private BattleCharacterData defaultData;    // 기본 캐릭터 데이터
    [SerializeField] private Status currentData;             // 현재 캐릭터 데이터

    [SerializeField] private GameObject hpBarObj;      // 유닛 체력바
    [SerializeField] private GameObject speedBarObj;   // 유닛의 턴
    private HpBar hpBar;
    private SpeedBar speedBar;

    private void Awake()
    {
        SetBattleData();
    }

    public void SetBattleData()
    {
        //defaultData = data;
        //currentData = defaultData.defaultStatus;

        SetHpBar();
        SetSpeedBar();

    }

    private void SetHpBar()
    {
        Vector3 hpPos = Camera.main.WorldToScreenPoint(this.transform.position);
        hpPos.y += 150.0f;
        hpBarObj = Instantiate(Resources.Load<GameObject>("BattleScenePrefab/HpBarBg"),
            GameObject.Find("Canvas").transform);
        hpBarObj.transform.position = hpPos;
        hpBar = hpBarObj.GetComponent<HpBar>();
        //hp.SetMaxHp = defaultData.defaultStatus.hp;
        //hp.UpdateHpBar(defaultData.defaultStatus.hp);
    }

    private void SetSpeedBar()
    {
        Vector3 speedPos = Camera.main.WorldToScreenPoint(this.transform.position);
        speedPos.y -= 150.0f;
        speedBarObj = Instantiate(Resources.Load<GameObject>("BattleScenePrefab/SpeedBarBg"),
            GameObject.Find("Canvas").transform);
        speedBarObj.transform.position = speedPos;
        speedBar = speedBarObj.GetComponent<SpeedBar>();
        //speed.SetSpeed = defaultData.defaultStatus.speed;
    }

    private void OnMouseOver()
    {
        BattleSceneManager.Instance.targeting = true;
        
        Debug.Log(this.name);
    }

    private void OnMouseExit()
    {
        BattleSceneManager.Instance.targeting = false;
    }

    /// <summary>
    /// 공격자로부터 데미지를 받을때 호출되는 함수
    /// </summary>
    private void GetDamage(Status caster, int damage)
    {
        int getDamage = (caster.atkPower + damage) - currentData.defPower;
        currentData.hp -= getDamage;
        hpBar.UpdateHpBar(currentData.hp);
    }
}
