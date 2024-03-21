using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[Serializable]
public struct Status
{
    public int hp;
    public int shield;
    public string name;
    public int atkPower;
    public int defPower;
    public int speed;
}

public abstract class BattleCharacterObj : MonoBehaviour
{
    public Status defaultStatus;
    public Status currentStatus;
    public GameObject hpBar;
    public GameObject speedBar;

    public abstract void CharacterInit();

    public virtual void SpeedBarInit()
    {
        if (speedBar == null)  // HP바가 없으면
        {
            Transform canvasPos = GameObject.Find("Canvas").GetComponent<Transform>();
            speedBar = Instantiate(Resources.Load<GameObject>("BattleScenePrefab/SpeedBarBg"), canvasPos);

            Vector3 speedBarPos = gameObject.transform.position;
            speedBarPos.y -= 1.5f;
            speedBar.transform.position = Camera.main.WorldToScreenPoint(speedBarPos);
            speedBar.GetComponent<SpeedBar>().SetSpeed = defaultStatus.speed;
        }
    }

    public virtual void HpbarInit()
    {
        if (hpBar == null)  // HP바가 없으면
        {
            Transform canvasPos = GameObject.Find("Canvas").GetComponent<Transform>();
            hpBar = Instantiate(Resources.Load<GameObject>("BattleScenePrefab/HpBarBg"), canvasPos);

            Vector3 hpBarPos = gameObject.transform.position;
            hpBarPos.y += 1.5f;
            hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPos);
            hpBar.GetComponent<HpBar>().SetMaxHp = defaultStatus.hp;
            hpBar.GetComponent<HpBar>().UpdateHpBar(defaultStatus.hp);
        }
    }
    public virtual void GetDamage(int value) { currentStatus.hp -= value; }
    public virtual void GiveDamage(BattleCharacterObj target) { target.GetDamage(currentStatus.atkPower); }
    public virtual void GetShield() { }
    public virtual void GetBuff() { }
    public virtual void Pattern() { }
}
