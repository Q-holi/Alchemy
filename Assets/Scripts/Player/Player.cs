using Unity.VisualScripting;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Direction playerDirection;
    [SerializeField] private float movementSpeed;

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

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    #region Unity Callbacks
    protected void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update()
    {
        #region Player Input
        if (!_playerInputIsDisabled)
        {
            ResetAnimationTriggers();//--플레이어 애니메이션 초기화
            PlayerMovementInput();//--InputData

            EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            false, false, false, false);
        }
        #endregion

        TestFunc();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    #endregion



    #region Custom Methods

    private void TestFunc()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene2_Field.ToString(), transform.position);
            //Debug.Log(Player.Instance.gameObject.name);
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

        // ??????怨룹꽑 ??????꾩렮維싧젆?
        if (xInput < 0)
            playerDirection = Direction.LEFT;
        else if (xInput > 0)
            playerDirection = Direction.RIGHT;
        else if (yInput < 0)
            playerDirection = Direction.DOWN;
        else if (yInput > 0)
            playerDirection = Direction.UP;
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





