using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryBar : MonoBehaviour
{
    [SerializeField] private UI_InventorySlot[] inventorySlot = null;
    [SerializeField] public Sprite blank16x16sprite;
    public GameObject inventoryBarDraggedItem;
    [HideInInspector] public GameObject inventoryTextBoxGameobject;

    #region Unity CallBack
    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }
    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }
    #endregion

    #region 인벤토리 슬롯 초기화
    private void ClearInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].inventorySlotImage.sprite = blank16x16sprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
                SetHighlightedInventorySlots(i);
            }
        }
    }
    #endregion
    /// <summary>
    /// inventoryLocation이 player면 플레이어인벤토리 리스트를 업데이트를 진행한다. 업데이트가 플레이어의 리스트인벤토리면 InventoryBar안에 있는 Slot를 모두 초기화 시킨다. 
    ///   if (inventorySlot.Length > 0 && inventoryList.Count > 0) inventoryList.Count -> inventoryList[player]의 길이가 0보다 작으면 
    ///   플레이어 인벤토리리스트에 아무것도 없기 때문에 Update진행 을 하지 않는다.
    ///   inventorySlot.Length -> inventorySlot 인스펙터창에서 하이어라키 안에 Inventory Bar 밑에 있는 Slot의 갯수 인스펙터에서 대입시켜주었습니다.
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="inventoryList"></param>
    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (inventoryLocation == InventoryLocation.player)
        {
            //--아이템 드랍이 이루어지면 인벤토리 칸을 바로 초기화 시킨다.
            ClearInventorySlots();

            if (inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                for (int i = 0; i < inventorySlot.Length; i++)
                {
                    if (i < inventoryList.Count)//--플에이어 인벤토리의 길이 (소유하고 있는 아이템 종류)가 이벤토리 슬롯보다 작으면 12번 다 검사하지 않고 넘어가기 위한 트리거
                    {
                        int itemCode = inventoryList[i].itemCode;

                        // ItemDetails itemDetails = InventoryManager. Instance.itemList.itemDetails.Find(x => x.itemCode == itemCode);
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                            //--플레이어 인벤토리리스트에 있는 itemCode로 가져오 itemDetail에 대한 내용을 인벤토리 슬롯에 내용을 입력한다.
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                            SetHighlightedInventorySlots(i);
                        }                          
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
    //--인벤토리 하이라이트(선택 테두리) 초기화
    public void ClearHighlightOnInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // loop through inventory slots and clear highlight sprites
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].isSelected)
                {
                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);
                    // Update inventory to show item as not selected
                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }
    }
    public void SetHighlightedInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // loop through inventory slots and clear highlight sprites
            for (int i = 0; i < inventorySlot.Length; i++)
                SetHighlightedInventorySlots(i);
        }

    }

    private void SetHighlightedInventorySlots(int itemPosition)
    {
        if (inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null)
        {
            if (inventorySlot[itemPosition].isSelected)//-- UI_InventorySlot 안에 있는 public bool isSelected가 true면 슬롯 하이라이트 활성화 false면 비활성화
            {
                inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);
                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.itemCode);
                //--▲▲▲ 현재 어떤 아이템(itemCode)가 선택되어 활성화(하이라이트)가 되어 있는지 설정
            }
        }     
    }
}