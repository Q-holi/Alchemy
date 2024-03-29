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
    [SerializeField] private TextMeshProUGUI defenseTxt;

    private int maxHp;

    public int SetMaxHp { set => maxHp = value; }
    public int SetDefensePower { set => defenseTxt.text = value.ToString(); }

    public void UpdateHpBar(int hp, int shield)
    {
        if (hp > maxHp)
            hp = maxHp;

        currentHpImg.fillAmount = hp / maxHp;
        currentShield.fillAmount = shield / maxHp;

        if(currentShield.fillAmount > 1)
            currentShield.fillAmount = 1;

        currentHpTxt.text = (hp + shield).ToString() + " / " + maxHp.ToString();
    }
}
