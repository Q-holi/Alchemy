using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBtn : MonoBehaviour
{
    public void BuildStack()
    {
        T_UIManager.Instance.StackGauge.GetComponent<T_PotionStackGauge>().BuildStack(1, Color.white);
    }
}
