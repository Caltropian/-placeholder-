using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerEvent : MonoBehaviour
{
    [SerializeField]
    private string[] validTags;
    public UnityEvent OnTriggerEnterExecute;
    public UnityEvent OnTriggerExitExecute;

    void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string validTag in validTags)
        {
            if (collision.CompareTag(validTag))
            {
                OnTriggerEnterExecute?.Invoke();
                return;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string validTag in validTags)
        {
            if (collision.CompareTag(validTag))
            {
                OnTriggerExitExecute?.Invoke();
                return;
            }
        }
    }
}
