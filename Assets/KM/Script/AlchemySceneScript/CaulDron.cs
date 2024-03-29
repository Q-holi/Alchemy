using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaulDron : MonoBehaviour
{
    [SerializeField] private Image contentAmount;               // 가마솥 내용물의 양(이미지)
    [SerializeField] private TextMeshProUGUI contentAmountTxt;  // 가마솥 내용물의 양(텍스트)
    [SerializeField] private List<BaseItemData> ingredientList;   // 가마솥에 넣은 재료 리스트

    [SerializeField] private int maxStack = 10;                 // 최대 가마솥의 수용량

    private void Start()
    {
        contentAmountTxt.text = "0 / " + maxStack.ToString();
    }

    /// <summary>
    /// 가마솥에 재료가 추가되면 관련 정보 업데이트
    /// </summary>
    public bool UpdateContent(int keyCode)
    {
        // 가마솥 수용량 체크
        if (ingredientList.Count < maxStack)
            ingredientList.Add(InventoryManager.itemDB[keyCode]);
        else
            return false;

        // 가마솥 수용량 업데이트
        contentAmount.fillAmount = (float)ingredientList.Count / (float)maxStack;
        contentAmountTxt.text = ingredientList.Count.ToString() + " / " + maxStack.ToString();

        // 가마솥 수용체 색 설정
        Color stackColor = new Color(0, 0, 0);
        foreach (BaseCollectionData ingredient in ingredientList)
        {
            float optiomAmount = ingredient.r + ingredient.b +
                                 ingredient.g + ingredient.a;

            stackColor.r += ingredient.r / optiomAmount;
            stackColor.g += ingredient.g / optiomAmount;
            stackColor.b += ingredient.b / optiomAmount;
        }
        contentAmount.color = new Color(stackColor.r / ingredientList.Count,
                                        stackColor.g / ingredientList.Count,
                                        stackColor.b / ingredientList.Count);
        return true;
    }
}
