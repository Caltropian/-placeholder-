using UnityEngine;

public abstract class IPausable : MonoBehaviour
{
    protected bool isGamePaused = false;
    public virtual void Pause(bool isPaused)
    {
        isGamePaused = isPaused;
    }
    protected virtual void OnEnable()
    {
        PauseManager.OnPaused += Pause;
    }
    protected virtual void OnDisable()
    {
        PauseManager.OnPaused -= Pause;
    }
}
