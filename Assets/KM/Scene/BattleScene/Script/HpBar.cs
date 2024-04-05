using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image currentHpImg;
    [SerializeField] private Image currentShield;
    [SerializeField] private TextMeshProUGUI currentHpTxt;
    [SerializeField] private TextMeshProUGUI currentDefTxt;

    private int maxHp;

    public int SetMaxHp { set => maxHp = value; }
    public int SetDef { set => currentDefTxt.text = value.ToString(); }

    public void UpdateHpBar(int hp, int shield)
    {
        if (hp > maxHp)
            hp = maxHp;

        currentHpImg.fillAmount = hp / maxHp;

        if(shield != 0) // 방어도가 있을때만 방어도를 표시
            currentShield.fillAmount = shield / maxHp;
        else
            currentShield.fillAmount = 0;

        if (currentShield.fillAmount > 1)
            currentShield.fillAmount = 1;

        currentHpTxt.text = (hp + shield).ToString() + " / " + maxHp.ToString();
    }
}