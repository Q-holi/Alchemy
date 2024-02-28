using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform inventoryArea;   // 인벤토리 영역
    [SerializeField] private GraphicRaycaster graphicRaycaster;     // 캔버스 레이캐스팅
    [SerializeField] private PointerEventData pointerEvent;         // 마우스 이벤트 데이터
    [SerializeField] private EventSystem eventSystem;               // UI 이벤트 인식 시스템 받아오기
    [SerializeField] private List<RaycastResult> rayList = new List<RaycastResult>();   // RayCast에 감지된 오브젝트 목록

    [SerializeField] private Transform itemInfo;        // 아이템 정보 출력창 위치
    [SerializeField] private InventoryData itemData;    // 인벤토리 아이템 데이터

    [SerializeField] private Transform stackGauge;      // 사용한 아이템의 스택 게이지 GUI
    [SerializeField] private TextMeshProUGUI stackCounter;      // 현재 스택을 숫자로 표시
    [SerializeField] private GameObject stackPrefab;        // 스택 게이지를 표시할 아이콘 프리팹
    private List<GameObject> stackList = new List<GameObject>();    // 현재 스택

    private GameObject tempImage;   // 복사될 오브젝트
    private Vector3 objPosition;    // Raycast hit 좌표
    [SerializeField] private GameObject bgPlane;           // Raycast 용 배경
    [SerializeField] private Collection selectItem;        // 선택된 아이템

    private void Start()
    {
        ShowItemList();
    }

    public Collection SelectItem
    {
        get { return selectItem; }
        set { selectItem = value; }
    }

    // 사용 가능한 아이템 리스트 출력
    public void ShowItemList()
    {
        GameObject slotPrefab = Resources.Load<GameObject>("IngredientSlot");

        foreach (Collection data in itemData.playerData.collection)
        {
            if (data.Keyvalue > 2000)
            {
                GameObject obj = Instantiate(slotPrefab, inventoryArea);
                obj.GetComponent<InventorySlot>().itemInit(data);
            }
        }
    }

    // 사용하려는 아이템 정보 출력
    public void ShowItemInfo()
    {
        itemInfo.GetComponent<ItemInfo>().ShowItemInfo(selectItem);
    }

    // 월드좌표 변환 함수
    private void ScreenToWorld()
    {
        // 레이캐스트 활용해서 좌표 추적
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (bgPlane.GetComponent<MeshCollider>().Raycast(ray, out hit, 1000))
        {
            objPosition = hit.point;
            objPosition.z = objPosition.z - 0.1f; // ZFighting 방지를 위해 z 위치 조정
        };
    }

    public GameObject Detectray()
    {
        pointerEvent = new PointerEventData(eventSystem);
        pointerEvent.position = Input.mousePosition;

        rayList.Clear();
        graphicRaycaster.Raycast(pointerEvent, rayList);

        if (rayList.Count > 0)
        {
            GameObject obj = rayList[0].gameObject;
            return obj;
        }
        else
            return null;
    }

    // 아이템 선택되었을때
    public void ItemSelected()
    {
        GameObject obj = Resources.Load<GameObject>("SelectItemObj");
        obj.GetComponent<SelectItem>().SetItemIcon(selectItem);
        ScreenToWorld();
        tempImage = Instantiate(obj, objPosition, Quaternion.identity);
    }

    // 아이템 드래그 중
    public void ItemDragging()
    {
        ScreenToWorld();
        tempImage.transform.position = objPosition;
    }

    // 아이템 드랍 시
    public void ItemDrop()
    {
        // UI 이벤트 영역에 걸리는 부분이 없는지 검사
        if (Detectray() != null)
        {
            if (Detectray().name == "Viewport") // Viewport 영역에 걸리면 사용 취소
            {
                Debug.Log("Item Use Cancel");
                Destroy(tempImage);
            }
        }

        tempImage.GetComponent<SelectItem>().ItemRigidbody.gravityScale = 10;
    }

    public void ShowStack(Collection item)
    {
        AddStack(item.black_Option, new Color(0, 0, 0));
        AddStack(item.blue_Option, new Color(0, 0, 1));
        AddStack(item.red_Option, new Color(1, 0, 0));

        stackCounter.text = (item.black_Option + item.blue_Option + item.red_Option).ToString() +
            " / ??";
        stackCounter.color = Color.white;
    }

    private void AddStack(int stackCount, Color stackColor)
    {
        for (int i = 0; i < stackCount; i++)
        {
            GameObject obj = Instantiate(stackPrefab, stackGauge.transform);
            obj.GetComponent<Image>().color = stackColor;
            stackList.Add(obj);
        }
    }

}
