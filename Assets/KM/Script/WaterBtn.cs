using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBtn : MonoBehaviour
{
    public void BuildStack()
    {
        UIManager.Instance.StackGauge.GetComponent<PotionStackGauge>().BuildStack(1, Color.black);
    }
}
