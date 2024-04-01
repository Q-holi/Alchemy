using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrderBtn : MonoBehaviour
{
    private ItemFilter filter;
    [SerializeField] private GameObject btnIcon;

    private void Awake()
    {
        filter = gameObject.GetComponentInParent<ItemFilter>();
    }

    // 버튼 클릭시 호출되는 이벤트
    public void OrderChange()
    {
        // 정렬타입 뒤집기
        filter.orderType = !filter.orderType;

        // 필터 변경 호출
        filter.SetFilter((int)filter.filter);

        // 아이콘 이미지 회전
        float rotateAngle = filter.orderType ? -180f : 180f;
        btnIcon.transform.Rotate(0f, 0f, rotateAngle); 
    }
}
