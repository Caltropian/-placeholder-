using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    public enum PlayerStates
    {
        UNDERWATER,
        ABOVEWATER
    }
    private PlayerStates _currState;
    public UnityEvent<PlayerStates> OnStateChange;

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
}
