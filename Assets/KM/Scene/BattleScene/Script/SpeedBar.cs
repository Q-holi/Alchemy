using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    [SerializeField] private Image myTurnGauge;
    private int speed;
    private float getTurnGauge = 100.0f;
    private float currentTurnGauge = 0;

    public bool isReady = false;

    public int SetSpeed { set => speed = value; }

    private void Update()
    {
        currentTurnGauge += Time.deltaTime * speed * BattleSceneManager.turnTimeScale;
        if (currentTurnGauge / getTurnGauge < 1.0f)
            myTurnGauge.fillAmount = currentTurnGauge / getTurnGauge;
        else
        {
            currentTurnGauge = getTurnGauge;
            isReady = true;
        }
    }
}
