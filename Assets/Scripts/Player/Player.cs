using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player>
{
    [SerializeField] private AnimationOverrides animationOverrides;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Direction playerDirection;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<CharacterAttribute> characterAttributeCustomisationList;
    [Tooltip("스프라이트 렌더러를 프리팹에 채워야 합니다")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer;
    private GridCursor gridCursor;
    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds useToolAnimationPause;

    private WaitForSeconds afterLiftToolAnimationPause;
    private WaitForSeconds liftToolAnimationPause;

    private WaitForSeconds afterPickAnimationPause;
    private WaitForSeconds pickAnimationPause;

    private bool playerToolUseDisabled = false;
    public bool PlayerToolUseDisabled { get => playerToolUseDisabled; set => playerToolUseDisabled = value; }

    private bool playerCollectDisabled = true; // 플레이어의 채집가능 여부
    public bool PlayerCollectDisabled { get => playerCollectDisabled; set => playerCollectDisabled = value; }

    #region Player Animation Parameter Variables
    private float xInput;
    private float yInput;
    private bool isCarrying = false;
    private bool isIdle;
    private bool isLiftingToolDown;
    private bool isLiftingToolLeft;
    private bool isLiftingToolRight;
    private bool isLiftingToolUp;
    private bool isRunning;
    private bool isUsingToolDown;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isUsingToolUp;
    private bool isSwingingToolDown;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isSwingingToolUp;
    private bool isWalking;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingLeft;
    private bool isPickingRight;
    private ToolEffect toolEffect = ToolEffect.none;
    #endregion

    private Camera camera;
    // Player attributes that can be swapped
    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    #region Unity Callbacks
    protected void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Arms, PartVariantColour.none, PartVariantType.none);
        // Initialise character attribute list
        characterAttributeCustomisationList = new List<CharacterAttribute>();
        camera = Camera.main;
    }
    private void Start()
    {
        gridCursor = FindObjectOfType<GridCursor>();

        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        pickAnimationPause = new WaitForSeconds(Settings.pickAnimationPause);

        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
        afterPickAnimationPause = new WaitForSeconds(Settings.afterPickAnimationPause);
    }

    private void Update()
    {
        #region Player Input
        if (!_playerInputIsDisabled)
        {
            ResetAnimationTriggers();//--플레이어 애니메이션 초기화
            PlayerMovementInput();//--InputData
            PlayerClickEvent();

            PlayerTestInput();

            EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            false, false, false, false);
        }
        #endregion

    }

    private void PlayerTestInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TimeManager.Instance.TestAdvanceGameMinute();

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TimeManager.Instance.TestAdvanceGameDay();
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    #endregion
    #region Custom Methods
    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // Apply base character arms customisation
        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        isCarrying = false;
    }
    /// <summary>
    /// 받은 item코드의 Sprite를 가져와 캐릭터 커스텀 속성 리스트에 추가 한다. 
    /// </summary>
    /// <param name="itemCode"></param>
    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            // Apply 'carry' character arms customisation
            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = true;
        }
    }

    private void ResetAnimationTriggers()
    {
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        toolEffect = ToolEffect.none;
    }

    private void PlayerMovementInput()
    {
        yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");

        //-- Player Idle State
        if (xInput == 0 && yInput == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
            return;
        }

        isRunning = true;
        isWalking = false;
        isIdle = false;
        movementSpeed = Settings.runSpeed;

        // 플레이어가 바라보는 방향
        if (xInput < 0)
            playerDirection = Direction.LEFT;
        else if (xInput > 0)
            playerDirection = Direction.RIGHT;
        else if (yInput < 0)
            playerDirection = Direction.DOWN;
        else if (yInput > 0)
            playerDirection = Direction.UP;
    }

    private void PlayerClickEvent()
    {
        if (!playerToolUseDisabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gridCursor.CursorIsEnabled)
                {
                    // Get Cursor Grid Position
                    Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();

                    // Get Player Grid Position
                    Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();
                    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
                }
            }
        }

    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        ResetMovement();

        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        // Get Selected  item details
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0))
                        ProcessPlayerClickInputSeed(gridPropertyDetails, itemDetails);
                    break;

                case ItemType.Commodity:
                    if (Input.GetMouseButtonDown(0))
                        ProcessPlayerClickInputCommodity(itemDetails);
                    break;
                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collecting_tool:
                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
                    break;
                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                default:
                    break;
            }
        }
    }

    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    HoeGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;
            case ItemType.Watering_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    WaterGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;
            case ItemType.Reaping_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    CollectPlant();
                }
                break;
            case ItemType.Collecting_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    CollectInPlayerDirection(gridPropertyDetails, itemDetails, playerDirection);
                }
                break;
            default:
                break;
        }
    }

    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        // Trigger animation
        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }

    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {

        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        // Set tool animation to hoe in override animation
        toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        if (playerDirection == Vector3Int.right)
            isUsingToolRight = true;
        else if (playerDirection == Vector3Int.left)
            isUsingToolLeft = true;
        else if (playerDirection == Vector3Int.up)
            isUsingToolUp = true;
        else if (playerDirection == Vector3Int.down)
            isUsingToolDown = true;

        yield return useToolAnimationPause;

        // 선택한 땅의 속성(gridProperty중 땅파기 옵션을 활성화 시킨다)
        if (gridPropertyDetails.daysSinceDug == -1)
            gridPropertyDetails.daysSinceDug = 0;

        // 선택한 해당 좌표를 업데이트 한다. 
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        // 호미로 지정한 그리드 타일 변경
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);

        // After animation pause
        yield return afterUseToolAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;


    }

    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        // Trigger animation
        StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }

    private IEnumerator WaterGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        // Set tool animation to watering can in override animation
        toolCharacterAttribute.partVariantType = PartVariantType.wateringCan;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        // TODO: If there is water in the watering can
        toolEffect = ToolEffect.watering;

        if (playerDirection == Vector3Int.right)
        {
            isLiftingToolRight = true;
        }
        else if (playerDirection == Vector3Int.left)
        {
            isLiftingToolLeft = true;
        }
        else if (playerDirection == Vector3Int.up)
        {
            isLiftingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down)
        {
            isLiftingToolDown = true;
        }

        yield return liftToolAnimationPause;

        // Set Grid property details for dug ground
        if (gridPropertyDetails.daysSinceWatered == -1)
        {
            gridPropertyDetails.daysSinceWatered = 0;
        }

        // Set grid property to dug
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);


        GridPropertiesManager.Instance.DisplayWaterGround(gridPropertyDetails);

        // After animation pause
        yield return afterLiftToolAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private void CollectInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        StartCoroutine(CollectInPlayerDirectionRoutine(gridPropertyDetails, equippedItemDetails, playerDirection));
    }

    private void CollectPlant()
    {
        if (playerCollectDisabled)
            return;

        Cell temp = new Cell();

        switch (EventHandler.CallGetTileType())
        {
            case TileType.FLOOR:
            case TileType.GRASS:
            case TileType.WALL:
                return;
            case TileType.NORMAL_COLLECT:
            case TileType.RARE_COLLECT:
            case TileType.EPIC_COLLECT:
            case TileType.LEGEND_COLLECT:
                PlayerInputIsDisabled = true;
                playerToolUseDisabled = true;
                StartCoroutine(SceneControllerManager.Instance.MiniGameSceneLoad());
                break;
        }
    }

    private IEnumerator CollectInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        ProcessCropWithEquippedItemInPlayerDirection(gridPropertyDetails, equippedItemDetails, playerDirection);

        yield return pickAnimationPause;

        // After animation pause
        yield return afterPickAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if (cursorGridPosition.x > playerGridPosition.x)
            return Vector3Int.right;
        else if (cursorGridPosition.x < playerGridPosition.x)
            return Vector3Int.left;
        else if (cursorGridPosition.y > playerGridPosition.y)
            return Vector3Int.up;
        else
            return Vector3Int.down;
    }

    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
            EventHandler.CallDropSelectedItemEvent();
    }

    private void ProcessPlayerClickInputSeed(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid && gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.seedItemCode == -1)
            PlantSeedAtCursor(gridPropertyDetails, itemDetails);
        else if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
            EventHandler.CallDropSelectedItemEvent();
    }

    private void ProcessCropWithEquippedItemInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        switch (equippedItemDetails.itemType)
        {
            case ItemType.Collecting_tool:
                if (playerDirection == Vector3Int.right)
                    isPickingRight = true;
                else if (playerDirection == Vector3Int.left)
                    isPickingLeft = true;
                else if (playerDirection == Vector3Int.up)
                    isPickingUp = true;
                else if (playerDirection == Vector3Int.down)
                    isPickingDown = true;

                break;

            case ItemType.none:
                break;
        }

        // Get crop at cursor grid location
        Crop crop = GridPropertiesManager.Instance.GetCropObjectAtGridLocation(gridPropertyDetails);

        // Execute Process Tool Action For crop
        if (crop != null)
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.Collecting_tool:
                    crop.ProcessToolAction(equippedItemDetails);
                    break;
            }
        }
    }

    private void PlantSeedAtCursor(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        //--해당 grid 위치에 정보중 씨앗 코드에 해당 아이템 코드를 업데이트 
        gridPropertyDetails.seedItemCode = itemDetails.itemCode;
        //--처음 씨앗을 심었기에 grid 속성 중 성장 일수를 0으로 카운트 시작 (초기값 -1)
        gridPropertyDetails.growthDays = 0;


        // 씨앗을 심은 화면 출력
        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);

        EventHandler.CallRemoveSelectedItemFromInventoryEvent();
    }

    private void ResetMovement()
    {
        // Reset movement
        xInput = 0f;
        yInput = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }
    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        ResetMovement();

        // Send event to any listeners for player movement input
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
        isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
        isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
        isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
        isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
        false, false, false, false);
    }
    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }
    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }
    private void PlayerMovement()
    {
        Vector2 direction = new Vector2(xInput, yInput);
        rigidbody2D.MovePosition(rigidbody2D.position + direction.normalized * (Time.fixedDeltaTime * movementSpeed));
    }
    #endregion
}
