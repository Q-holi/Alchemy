using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image itemFrame;               // 아이템 프레임
    [SerializeField] private Image itemIcon;                // 아이템 아이콘
    [SerializeField] private TextMeshProUGUI itemName;      // 아이템 이름
    [SerializeField] private TextMeshProUGUI itemRank;      // 아이템 등급
    [SerializeField] private GameObject itemOption;         // 아이템 옵션표기
    [SerializeField] private TextMeshProUGUI itemDetail;    // 아이템 설명
    [SerializeField] private GameObject stackPrefab;        // 아이템 옵션 표기를 위한 스택 프리팹

    [SerializeField] private Inventory inventory;       // 인벤토리 정보 불러오기
    private List<GameObject> stackList = new List<GameObject>();    // 아이템 옵션 리스트

    private void Awake()
    {
        // 시작시 아이템 정보창 비우기
        itemFrame.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        itemRank.gameObject.SetActive(false);
        itemOption.gameObject.SetActive(false);
        itemDetail.gameObject.SetActive(false);
    }

    private void Start()
    {
        InventoryEventHandler.OnMouse += ShowItemInfo;
    }
    private void OnDestroy()
    {
        InventoryEventHandler.OnMouse -= ShowItemInfo;
    }

    private void Update()
    {
    }

    public void ShowItemInfo(Item info)
    {
        // 아이템 정보 표시를 위해 정보창 정보 visible
        itemFrame.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        itemRank.gameObject.SetActive(true);
        itemOption.gameObject.SetActive(true);
        itemDetail.gameObject.SetActive(true);

        // 아이템 정보 설정
        itemFrame.color = UtilFunction.GetColor(info.itemData.rating);
        itemIcon.sprite = info.itemData.sprite;
        itemName.text = info.itemData.itemName;
        itemName.color = UtilFunction.GetColor(info.itemData.rating);
        itemRank.text = info.itemData.rating.ToString();
        itemRank.color = UtilFunction.GetColor(info.itemData.rating);
        itemDetail.text = info.itemData.detail;

        // 스택 출력전, 스택 리스트 초기화
        foreach (GameObject stack in stackList)
            Destroy(stack);
        stackList.Clear();

        Collection temp = (Collection)info;
        BuildStack((int)temp.options.x, Color.red);
        BuildStack((int)temp.options.y, Color.green);
        BuildStack((int)temp.options.z, Color.blue);
        BuildStack((int)temp.options.w, Color.white);
    }

    private void BuildStack(int count, Color color)
    {
        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(stackPrefab, itemOption.transform);
            temp.GetComponent<Image>().color = color;
            stackList.Add(temp);
        }
    }
}
