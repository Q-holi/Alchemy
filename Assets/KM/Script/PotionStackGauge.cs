using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionStackGauge : MonoBehaviour
{
    [SerializeField] private GameObject stackCounter;
    [SerializeField] private TextMeshProUGUI stackCounterTxt;
    public List<GameObject> stackList = new List<GameObject>();
    private int currentStack = 0;
    private int maxStack = 20;
    public int MasStack { get => maxStack; set => maxStack = value; }

    private void Awake()
    {
        stackCounterTxt.text = currentStack.ToString() + " / " + maxStack.ToString();
    }

    public void CountStack(Collection item)
    {
        BuildStack(item.Red_Option, Color.red);
        BuildStack(item.Green_Option, Color.green);
        BuildStack(item.Blue_Option, Color.blue);
    }

    public void BuildStack(int count, Color color)
    {
        for (int i = 0; i < count; i++)
        {
            if (currentStack < maxStack)
            {
                currentStack++;
                GameObject temp = Instantiate(UIManager.Instance.StackPrefab, stackCounter.transform);
                temp.GetComponent<Image>().color = color;
                stackList.Add(temp);
            }
            else
                break;
        }
        stackCounterTxt.text = currentStack.ToString() + " / " + maxStack.ToString();
        UIManager.Instance.CaulDron.GetComponent<CaulDron>().UpdateContent(currentStack, maxStack, stackList);
    }
}
