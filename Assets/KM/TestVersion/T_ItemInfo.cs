using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class T_ItemInfo : MonoBehaviour
{
    [SerializeField] private Image itemFrame;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemRank;
    [SerializeField] private GameObject itemOption;
    [SerializeField] private TextMeshProUGUI itemDetail;
    private List<GameObject> stackList = new List<GameObject>();

    private void Awake()
    {
        itemFrame.gameObject.SetActive(false);
        itemIcon.gameObject.SetActive(false);
        itemName.gameObject.SetActive(false);
        itemRank.gameObject.SetActive(false);
        itemOption.gameObject.SetActive(false);
        itemDetail.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (T_UIManager.Instance.SelectItem != null)
            ShowItemInfo((Collection)T_UIManager.Instance.SelectItem);
        else
            return;
    }

    public void ShowItemInfo(Collection info)
    {
        itemFrame.gameObject.SetActive(true);
        itemIcon.gameObject.SetActive(true);
        itemName.gameObject.SetActive(true);
        itemRank.gameObject.SetActive(true);
        itemOption.gameObject.SetActive(true);
        itemDetail.gameObject.SetActive(true);

        itemFrame.color = T_UIManager.Instance.GetColor(info.Rating);
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(info.Texture2DImagePath);
        itemName.text = info.Name;
        itemName.color = T_UIManager.Instance.GetColor(info.Rating);
        itemRank.text = info.Rating.ToString();
        itemRank.color = T_UIManager.Instance.GetColor(info.Rating);

        foreach (GameObject stack in stackList)
            Destroy(stack);
        stackList.Clear();

        BuildStack(info.Red_Option, Color.red);
        BuildStack(info.Green_Option, Color.green);
        BuildStack(info.Blue_Option, Color.blue);
    }

    private void BuildStack(int count, Color color)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(T_UIManager.Instance.StackPrefab, itemOption.transform);
            temp.GetComponent<Image>().color = color;
            stackList.Add(temp);
        }
    }
}
