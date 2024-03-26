using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UtilFunction
{
    // 파라미터로 넘겨받은 UI와 GraphicRayCasting 검사
    public static bool Detectray(string name)
    {
        // 캔버스 레이캐스팅
        GraphicRaycaster graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        // UI 이벤트 인식 시스템 받아오기
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        // RayCast에 감지된 오브젝트 목록
        List<RaycastResult> rayList = new List<RaycastResult>();

        // 마우스 이벤트 데이터
        PointerEventData pointerEvent = new PointerEventData(eventSystem);
        pointerEvent.position = Input.mousePosition;

        // 감지 전, 감지된 오브젝트 목록 초기화
        rayList.Clear();
        // 레이캐스팅 된 오브젝트들을 rayList 에 추가
        graphicRaycaster.Raycast(pointerEvent, rayList);

        for (int i = 0; i < rayList.Count; i++)
        {
            if (rayList[i].gameObject.name == name)
                return true;
        }
        return false;
    }

    // 화면좌표를 오브젝트 좌표로 변환
    public static Vector3 ScreenToWorldPos()
    {
        GameObject plane = GameObject.Find("RayCastBG_Obj");

        // 레이캐스트 활용해서 좌표 추적
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 objPosition = new Vector3();

        if (plane.GetComponent<MeshCollider>().Raycast(ray, out hit, 1000))
        {
            objPosition = hit.point;
            objPosition.z = objPosition.z - 0.1f; // ZFighting 방지를 위해 z 위치 조정
            return objPosition;
        }
        else
            return objPosition;
    }

    // 등급에 맞는 색 추출
    public static Color GetColor(Rating rating)
    {
        switch (rating)
        {
            case Rating.Normal:
                return Color.white;
            case Rating.Rare:
                return Color.blue;
            case Rating.Epic:
                return Color.magenta;
            case Rating.Legend:
                return Color.yellow;
        }
        return Color.white;
    }

    // 아이템의 유형을 검사해서 자동으로 알맞는 아이템 타입으로 다운 캐스팅
    public static Item InventoryItemTypeFilter(Item item, InventoryFilterType filter)
    {
        switch (filter)
        {
            case InventoryFilterType.Collection when item is Collection collection:
                return collection;
            case InventoryFilterType.Potion when item is Potion potion:
                return potion;
            case InventoryFilterType.Tool when item is Tool tool:
                return tool;
            default:
                return null;
        }
    }
}
