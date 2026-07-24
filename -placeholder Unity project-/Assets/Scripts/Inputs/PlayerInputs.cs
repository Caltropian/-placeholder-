using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : IInputReciever
{
    private InputSystem_Actions.PlayerActions playerActions;

    #region Bindings
    private InputAction playerMovement;
    private InputAction sprint;
    [SerializeField]
    private SwimMovement swimMovement;
    [SerializeField]
    private WalkMovement walkMovement;
    [SerializeField]
    private PlungeMovement plungeMovement;
    #endregion

    private bool _isUnderwater = false;
    [SerializeField]
    private bool _canPlunge;
    public bool CanPlunge
    {
        get
        {
            return _canPlunge;
        }
        set
        {
            _canPlunge = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        playerActions = inputActions.Player;
        playerMovement = playerActions.Move;
        sprint = playerActions.Sprint;
        CanPlunge = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (!isGamePaused)
        {
            playerActions.Enable();
            sprint.performed += BoostInput;
            sprint.canceled += CancelledBoostInput;

        }
    }

    private void BoostInput(InputAction.CallbackContext context)
    {
        if (_isUnderwater)
        {
            swimMovement.Boost(true);
        }
    }
    private void CancelledBoostInput(InputAction.CallbackContext context)
    {

        swimMovement.Boost(false);

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (!isGamePaused)
        {
            sprint.performed -= BoostInput;
            sprint.canceled -= CancelledBoostInput;
            playerActions.Disable();
            swimMovement.IsBoosting = false;
            swimMovement.IsMoving = false;
            walkMovement.IsMoving = false;
        }
    }
    public override void Pause(bool isPaused)
    {
        if (isPaused)
        {
            sprint.performed -= BoostInput;
            sprint.canceled -= CancelledBoostInput;
            playerActions.Disable();
        }
        else
        {
            sprint.performed += BoostInput;
            sprint.canceled += CancelledBoostInput;
            playerActions.Enable();
        }
    }
    void Update()
    {
        Vector2 movementAxis = playerMovement.ReadValue<Vector2>();
        if (CanPlunge && movementAxis.y < 0)
        {
            plungeMovement.Plunge(movementAxis);
        }
        else if (_isUnderwater)
        {
            swimMovement.Move(playerMovement.IsPressed(), movementAxis);
        }
        else
        {
            walkMovement.Move(playerMovement.IsPressed(), movementAxis);
        }
    }

    public void OnChangedPlayerState(PlayerState.PlayerStates state)
    {
        if (state == PlayerState.PlayerStates.UNDERWATER)
        {
            _isUnderwater = true;
            walkMovement.enabled = false;
            swimMovement.enabled = true;
        }
        else
        {
            _isUnderwater = false;
            walkMovement.enabled = true;
            swimMovement.enabled = false;
        }
    }
}
