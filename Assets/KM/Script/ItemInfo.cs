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

    [SerializeField] private InventoryList inventory;       // 인벤토리 정보 불러오기
    private IItem item;    // 중복 검증을 위한 아이템 데이터
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

    private void Update()
    {
        // 선택한 아이템이 있으면, 아이템 정보 출력
        if (inventory.SelectItem != null &&
            inventory.SelectItem != item)
            ShowItemInfo((Collection)inventory.SelectItem);
        else
            return;
    }

    public void ShowItemInfo(IItem info)
    {
        item = info;
        // 아이템 정보 표시를 위해 정보창 정보 visible
        itemFrame.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        itemRank.gameObject.SetActive(true);
        itemOption.gameObject.SetActive(true);
        itemDetail.gameObject.SetActive(true);

        // 아이템 정보 설정
        itemFrame.color = UtilFunction.GetColor(info.Rating);
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempIcons").GetSprite(info.Texture2DImagePath);
        itemName.text = info.Name;
        itemName.color = UtilFunction.GetColor(info.Rating);
        itemRank.text = info.Rating.ToString();
        itemRank.color = UtilFunction.GetColor(info.Rating);

        // 스택 출력전, 스택 리스트 초기화
        foreach (GameObject stack in stackList)
            Destroy(stack);
        stackList.Clear();

        Collection temp = (Collection)info;
        BuildStack(temp.Red_Option, Color.red);
        BuildStack(temp.Green_Option, Color.green);
        BuildStack(temp.Blue_Option, Color.blue);
        BuildStack(temp.Alpha_Option, Color.white);
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
