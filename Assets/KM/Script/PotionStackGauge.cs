using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionStackGauge : MonoBehaviour
{
    [SerializeField] private GameObject stackCounter;
    [SerializeField] private TextMeshProUGUI stackCounterTxt;
    private List<GameObject> stackList = new List<GameObject>();

    public void CountStack(Collection item)
    {
        BuildStack(item.Red_Option, Color.red);
        BuildStack(item.Black_Option, Color.black);
        BuildStack(item.Blue_Option, Color.blue);
    }

    private void BuildStack(int count, Color color)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(UIManager.Instance.StackPrefab, stackCounter.transform);
            temp.GetComponent<Image>().color = color;
            stackList.Add(temp);
        }
    }
}
