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
    #endregion
    protected override void Awake()
    {
        base.Awake();
        playerActions = inputActions.Player;
        playerMovement = playerActions.Move;
        sprint = playerActions.Sprint;
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
        swimMovement.Boost(true);
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
        swimMovement.Move(playerMovement.IsPressed(), movementAxis);
    }

}
