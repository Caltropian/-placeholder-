using UnityEngine;

public abstract class IInputReciever : IPausable
{
    protected InputSystem_Actions inputActions;
    protected virtual void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
}

