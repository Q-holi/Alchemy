using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private BattleCharacterData defaultData;    // 기본 캐릭터 데이터
    [SerializeField] private Status currentData;             // 현재 캐릭터 데이터

    [SerializeField] private GameObject hpBarObj;      // 유닛 체력바
    [SerializeField] private GameObject speedBarObj;   // 유닛의 턴
    [SerializeField] private GameObject highLight;     // 유닛 선택 강조표시 이미지

    private HpBar hpBar;
    private SpeedBar speedBar;
    private Dictionary<string, Buff> buffList;       // 현재 받고있는 버프 리스트

    /// <summary>
    /// 유닛의 이름으로 DB에서 데이터를 가져오기
    /// </summary>
    public void SetBattleData(string name)
    {
        defaultData = BattleSceneManager.unitDB[name];

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
        if (!BattleSceneManager.Instance.targeting)
            BattleSceneManager.Instance.targeting = true;
        else
            return;
        
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

        // 보호막이 있으면
        if (currentData.shield > 0)
        {
            // 남는데미지가 있는지 계산
            int remainDamage = currentData.shield - getDamage;

            if (remainDamage > 0)   // 보호막을 뛰어넘는 데미지 일때
            {
                currentData.shield = 0;
                currentData.hp -= remainDamage;
            }
            else // 보호막이 충분할때
                currentData.shield -= getDamage;
        }

        hpBar.UpdateHpBar(currentData.hp, currentData.shield);
    }

    /// <summary>
    /// 회복할때 호출되는 함수
    /// </summary>
    private void GetHeal(Status caster, int amount)
    {
        caster.hp += amount;
    }

    /// <summary>
    /// 보호막(방어도) 를 얻을때 호출되는 함수
    /// </summary>
    private void GetShield(Status caster, int amount)
    { 
        caster.shield += amount;
    }

    private void CheckBuff()
    { 
    
    }
}
