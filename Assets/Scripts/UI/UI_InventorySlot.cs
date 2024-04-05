using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class UI_InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region 변수
    private Camera _camera;
    private Canvas parentCanvas;
    private Transform parentItem;
    private GameObject draggedItem;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;


    [SerializeField] private UI_InventoryBar inventoryBar = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;
    [SerializeField] private GameObject itemPrefab = null;
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;
    [SerializeField] private int slotIndex;
    [HideInInspector] public bool isSelected = false;
    #endregion

    #region Unity CallBack
    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }
    private void Start()
    {
        _camera = Camera.main;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }
    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            // Disable keyboard input
            //Player.Instance.DisablePlayerInputAndResetMovement();

            // Instatiate gameobject as dragged item
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            // Get image for dragged item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
            draggedItem.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            //--드래그 아이템을 인벤토리 Bar가 아닌 Bar안의 이동 시켰을때 
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UI_InventorySlot>() != null)
            {
                int toslotIndex = eventData.pointerCurrentRaycast.gameObject.GetComponent<UI_InventorySlot>().slotIndex;

                //--인벤토리Bar의 아이템을 지정한 곳의 아이템과 스왑
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotIndex, toslotIndex);

                DestroyInventoryTextBox();
                ClearSelectedItem();
            }
            // else attempt to drop the item if it can be dropped
            else
            {
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }
            // Enable player input
            //Player.Instance.EnablePlayerInput();
        }
    }
    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z));

            Vector3Int gridPosition = GridPropertiesManager.Instance.grid.WorldToCell(worldPosition);
            GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(gridPosition.x, gridPosition.y);

            if (gridPropertyDetails != null && gridPropertyDetails.canDropItem)
            {
                // Create item from prefab at mouse position
                GameObject itemGameObject = Instantiate(itemPrefab, new Vector3(worldPosition.x, worldPosition.y - Settings.gridCellSize / 2.0f,worldPosition.z), Quaternion.identity, parentItem);
                Item item = itemGameObject.GetComponent<Item>();
                item.ItemCode = itemDetails.itemCode;

                // Remove item from players inventory
                InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);

                // If no more of item then clear selected
                if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, item.ItemCode) == -1)
                    ClearSelectedItem();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text box with item details
        if (itemQuantity != 0)
        {

            // Instantiate inventory text box
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UI_InventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<UI_InventoryTextBox>();

            // Set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // Populate text box
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");


            inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
            inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);

            // Set text box position according to inventory bar position
            //if (inventoryBar.IsInventoryBarPositionBottom)
            //{
            //   
            //}
            //else
            //{
            //    inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            //    inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            //}
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }
    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameobject != null)
            Destroy(inventoryBar.inventoryTextBoxGameobject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // if inventory slot currently selected then deselect
            if (isSelected == true)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    private void SetSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        // Highlight item on inventory bar
        isSelected = true;

        // Set highlighted inventory slots
        inventoryBar.SetHighlightedInventorySlots();

        // Set item selected in inventory
        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);
    }

    private void ClearSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        // set no item selected in inventory
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
    }

    public void SceneLoaded()
    {
     //   parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }
}

