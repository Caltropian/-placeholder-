using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : IInputReciever
{
    public static event Action<bool> OnPaused;
    private InputSystem_Actions.UIActions uiActions;
    private InputAction pause;
    [SerializeField]
    private GameObject[] pauseVisuals;

    protected override void Awake()
    {
        base.Awake();
        uiActions = inputActions.UI;
        pause = uiActions.Cancel;
        HandleVisuals(false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        pause.Enable();
        pause.started += PauseGame;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        pause.started -= PauseGame;
        pause.Disable();
    }
    public void PauseGame(InputAction.CallbackContext context)
    {
        OnPaused?.Invoke(!isGamePaused);
    }
    public void PauseGame()
    {
        OnPaused?.Invoke(!isGamePaused);
    }
    public void PauseGame(bool value)
    {
        OnPaused?.Invoke(value);
    }
    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPaused)
            Physics2D.simulationMode = SimulationMode2D.Script;
        else
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        HandleVisuals(isPaused);

    }
    private void HandleVisuals(bool isPaused)
    {
        foreach (GameObject pauseVisual in pauseVisuals)
        {
            pauseVisual.SetActive(isPaused);
        }
    }

}
