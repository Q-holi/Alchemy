using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System;
using UnityEngine.EventSystems;

[System.Serializable]
public class UIManager : Singleton<UIManager>
{
    // 캔버스 레이캐스팅
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    // 마우스 이벤트 데이터
    [SerializeField] private PointerEventData pointerEvent;
    // UI 이벤트 인식 시스템 받아오기
    [SerializeField] private EventSystem eventSystem;
    // RayCast에 감지된 오브젝트 목록
    [SerializeField] private List<RaycastResult> rayList = new List<RaycastResult>();

    [SerializeField] private Transform itemInfo;        // 아이템 정보 출력창
    public Transform Iteminfo { get => itemInfo; }
    [SerializeField] private IItem selectItem;        // 선택된 아이템
    public IItem SelectItem { get => selectItem; set => selectItem = value; }
    [SerializeField] private GameObject stackPrefab;      // 재료 스택을 표시할 아이콘프리팹
    public GameObject StackPrefab { get => stackPrefab; }

    [SerializeField] private GameObject bgPlane;        // 레이캐스팅용 배경 오브젝트
    [SerializeField] private GameObject selectItemPrefab;   // 복사될 오브젝트
    public GameObject SelectItemPrefab { get => selectItemPrefab; }

    public Vector3 ScreenToWorldPos()
    {
        // 레이캐스트 활용해서 좌표 추적
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 objPosition = new Vector3();

        if (bgPlane.GetComponent<MeshCollider>().Raycast(ray, out hit, 1000))
        {
            objPosition = hit.point;
            objPosition.z = objPosition.z - 0.1f; // ZFighting 방지를 위해 z 위치 조정
            return objPosition;
        }
        else
            return objPosition;
    }

    public GameObject Detectray()
    {
        pointerEvent = new PointerEventData(eventSystem);
        pointerEvent.position = Input.mousePosition;

        // 감지 전, 감지된 오브젝트 목록 초기화
        rayList.Clear();
        // 레이캐스팅 된 오브젝트들을 rayList 에 추가
        graphicRaycaster.Raycast(pointerEvent, rayList);

        if (rayList.Count > 0)
        {
            GameObject obj = rayList[0].gameObject;
            return obj;
        }
        else
            return null;
    }

    public Color GetColor(Rating rating)
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
}
