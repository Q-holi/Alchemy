using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    [SerializeField] private Image myTurnGauge;
    private int speed;
    private float currentTurnGauge = 0;

    public int SetSpeed { set => speed = value; }

    private void Update()
    {
        currentTurnGauge += Time.deltaTime * speed;
        if (currentTurnGauge / 100.0f <= 1.0f)
            myTurnGauge.fillAmount = currentTurnGauge / 100.0f;
        else
            currentTurnGauge = 0;
    }
}
