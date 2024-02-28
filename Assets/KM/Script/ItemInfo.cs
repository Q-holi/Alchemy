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

    public void ShowItemInfo(Item info)
    {
        itemIcon.sprite = Resources.Load<SpriteAtlas>("TempOreImage").GetSprite(info.texture2DImagePath);
        itemName.text = info.name;
        itemRank.text = info.rating.ToString();
    }
}
