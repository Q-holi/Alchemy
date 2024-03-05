using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image itemFrame;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemRank;
    [SerializeField] private GameObject itemOption;
    [SerializeField] private TextMeshProUGUI itemDetail;
    private List<GameObject> stackList = new List<GameObject>();

    private void Update()
    {
        if (UIManager.Instance.SelectItem != null)
            ShowItemInfo((Collection)UIManager.Instance.SelectItem);
        else
            return;
    }

    public void ShowItemInfo(Collection info)
    {
        itemFrame.color = UIManager.Instance.GetColor(info.Rating);
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(info.Texture2DImagePath);
        itemName.text = info.Name;
        itemName.color = UIManager.Instance.GetColor(info.Rating);
        itemRank.text = info.Rating.ToString();
        itemRank.color = UIManager.Instance.GetColor(info.Rating);

        foreach (GameObject stack in stackList)
            Destroy(stack);
        stackList.Clear();

        BuildStack(info.Red_Option, Color.red);
        BuildStack(info.Black_Option, Color.black);
        BuildStack(info.Blue_Option, Color.blue);
    }

    private void BuildStack(int count, Color color)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject temp = Instantiate(UIManager.Instance.StackPrefab, itemOption.transform);
            temp.GetComponent<Image>().color = color;
            stackList.Add(temp);
        }
    }
}
