using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemRank;
    [SerializeField] private GameObject itemOption;
    [SerializeField] private TextMeshProUGUI itemDetail;
    [SerializeField] private GameObject stackPrefab;

    private List<GameObject> stackList = new List<GameObject>();

    public void ShowItemInfo(Collection info)
    {
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(info.Texture2DImagePath);
        itemName.text = info.Name;
        itemRank.text = info.Rating.ToString();
        switch (info.Rating.ToString())
        {
            case "Normal":
                itemRank.color = new Color(1.0f, 1.0f, 1.0f);
                break;
            case "Rare":
                itemRank.color = new Color(17 / 255.0f, 68 / 255.0f, 187 / 255.0f);
                break;
            case "Epic":
                itemRank.color = new Color(138 / 255.0f, 43 / 255.0f, 226 / 255.0f);
                break;
            case "Legend":
                itemRank.color = new Color(255 / 255.0f, 127 / 255.0f, 0 / 255.0f);
                break;
        }

        foreach (GameObject obj in stackList)
            Destroy(obj);

        stackList.Clear();
        ShowStack(info.black_Option, new Color(0, 0, 0));
        ShowStack(info.blue_Option, new Color(0, 0, 1));
        ShowStack(info.red_Option, new Color(1, 0, 0));
    }

    private void ShowStack(int stackCount, Color stackColor)
    {
        for (int i = 0; i < stackCount; i++)
        {
            GameObject obj = Instantiate(stackPrefab, itemOption.transform);
            obj.GetComponent<Image>().color = stackColor;
            stackList.Add(obj);
        }
    }
}
