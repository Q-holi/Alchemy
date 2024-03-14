using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaulDron : MonoBehaviour
{
    [SerializeField] private Image contentAmount;               // 가마솥 내용물의 양(이미지)
    [SerializeField] private TextMeshProUGUI contentAmountTxt;  // 가마솥 내용물의 양(텍스트)

    [SerializeField] private int maxStack = 10;                 // 최대 가마솥의 수용량

    private void Start()
    {
        contentAmountTxt.text = "0 / " + maxStack.ToString();
    }

    public bool UpdateContent(Collection item)
    {
        List<Collection> ingredientList = AlchemyManager.instance.IngredientList;
        // 가마솥 수용량 체크
        if (ingredientList.Count < maxStack)
            ingredientList.Add(item);
        else
            return false;

        // 가마솥 수용량 업데이트
        contentAmount.fillAmount = (float)ingredientList.Count / (float)maxStack;
        contentAmountTxt.text = ingredientList.Count.ToString() + " / " + maxStack.ToString();

        // 가마솥 수용체 색 설정
        Color stackColor = new Color(0, 0, 0);
        foreach (Collection ingredient in ingredientList)
        {
            int optiomAmount = ingredient.Red_Option + ingredient.Green_Option + ingredient.Blue_Option + ingredient.Alpha_Option;

            stackColor.r += (float)ingredient.Red_Option / (float)optiomAmount;
            stackColor.g += (float)ingredient.Green_Option / (float)optiomAmount;
            stackColor.b += (float)ingredient.Blue_Option / (float)optiomAmount;
        }
        contentAmount.color = new Color(stackColor.r / ingredientList.Count,
                                        stackColor.g / ingredientList.Count,
                                        stackColor.b / ingredientList.Count);
        return true;
    }
}
