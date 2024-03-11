using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_CaulDron : MonoBehaviour
{
    [SerializeField] private Image cauldron;
    [SerializeField] private Image contentAmount;

    public void UpdateContent(int current, int max, List<GameObject> stacks)
    {
        contentAmount.fillAmount = (float)current / (float)max;

        Color stackColor = new Color(0, 0, 0);
        foreach (GameObject stack in stacks)
        {
            stackColor += stack.GetComponent<Image>().color;
        }
        contentAmount.color = new Color(stackColor.r / stacks.Count, stackColor.g / stacks.Count, stackColor.b / stacks.Count, contentAmount.fillAmount);
    }
}
