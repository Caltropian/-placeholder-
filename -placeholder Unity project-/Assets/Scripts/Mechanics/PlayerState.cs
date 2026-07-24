using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerState : MonoBehaviour
{
    public enum PlayerStates
    {
        UNDERWATER,
        ABOVEWATER
    }
    private PlayerStates _currState;
    public UnityEvent<PlayerStates> OnStateChange;
    public Transform CurrentCheckpoint;
    [Header("Dependencies for Initial State")]
    [SerializeField]
    private OxygenTracker oxygenTracker;
    private Rigidbody2D rb2d;
    [SerializeField]
    private PlayerInputs playerInputs;
    void Awake()
    {
        oxygenTracker = oxygenTracker != null ? oxygenTracker : GetComponentInChildren<OxygenTracker>();
        playerInputs = playerInputs != null ? playerInputs : GetComponentInChildren<PlayerInputs>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        OxygenTracker.OnDrown += ResetToInitialState;
        DeathScreenVisuals.OnScreenClear += ActivateInputs;
    }
    void OnDisable()
    {
        OxygenTracker.OnDrown -= ResetToInitialState;
        DeathScreenVisuals.OnScreenClear -= ActivateInputs;
    }

    public PlayerStates CurrentState
    {
        set
        {
            _currState = value;
            OnStateChange?.Invoke(_currState);
        }
        get
        {
            return _currState;
        }
    }
    private void ResetToInitialState()
    {
        oxygenTracker.ResetState();
        this.rb2d.linearVelocity = new(0, 0);
        this.rb2d.rotation = 0;
        this.rb2d.totalTorque = 0;
        CurrentState = PlayerStates.ABOVEWATER;
        playerInputs.CanPlunge = false;
        this.transform.position = new(CurrentCheckpoint.position.x, CurrentCheckpoint.position.y, 0);
        playerInputs.enabled = false;
    }
    //this will cause a bug.
    private void ActivateInputs()
    {
        playerInputs.enabled = true;
    }
}
