using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UtilFunction
{
    /// <summary>
    /// 파라미터로 넘겨받은 UI의 GraphicRayCasting 검사
    /// </summary>
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

    /// <summary>
    /// 화면좌표(NDC)를 오브젝트 좌표(World)로 변환
    /// </summary>
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

    /// <summary>
    /// 등급에 맞는 색 추출
    /// </summary>
    public static Color GetColor(ItemRating rating)
    {
        switch (rating)
        {
            case ItemRating.Normal:
                return Color.white;
            case ItemRating.Rare:
                return Color.blue;
            case ItemRating.Epic:
                return Color.magenta;
            case ItemRating.Legend:
                return Color.yellow;
        }
        return Color.white;
    }
}
