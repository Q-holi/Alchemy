using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image currentHpImg;
    [SerializeField] private TextMeshProUGUI currentHpTxt;
    private int maxHp;
    public int SetMaxHp { set => maxHp = value; }

    public void UpdateHpBar(int hp)
    {
        currentHpImg.fillAmount = hp / maxHp;
        currentHpTxt.text = hp.ToString() + " / " + maxHp.ToString();
    }
}
