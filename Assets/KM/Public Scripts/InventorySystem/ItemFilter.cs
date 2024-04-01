using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ItemFilter : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown filterOptions;
    [SerializeField] private Button orderBtn;

    public ItemFilterType filter;   // 아이템 필터 유형
    public bool orderType;   // true 내림차순, false 내림차순

    private void Start()
    {
        filterOptions.value = (int)filter;

        // DropDown 에 있는 OnValueChanged 이벤트에 setDropDown 이벤트 추가
        filterOptions.onValueChanged.AddListener(delegate { SetFilter(filterOptions.value); });
    }

    /// <summary>
    /// 필터 세팅
    /// </summary>
    public void SetFilter(int option)
    {
        filter = (ItemFilterType)option;
        InventoryEventHandler.OnUseFilter(option, orderType);
    }
}
