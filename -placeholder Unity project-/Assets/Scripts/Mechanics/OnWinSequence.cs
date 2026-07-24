using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnWinSequence : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    DeathScreenVisuals blackScreenVisuals;

    [Header("Settings")]
    [SerializeField]
    private float timeToFadeWinScreen;
    [SerializeField]
    private bool willItFadeToBlack;

    public UnityEvent OnWinExecute;


    public void Win()
    {
        if (!willItFadeToBlack)
        {
            OnWinExecute?.Invoke();
            return;
        }
        else
        {
            StartCoroutine(FadeToBlack(timeToFadeWinScreen, blackScreenVisuals));
        }
    }
    private IEnumerator FadeToBlack(float timeToFade, DeathScreenVisuals blackScreenManager)
    {
        float localTimer = timeToFade;
        while (localTimer >= 0)
        {
            localTimer -= Time.deltaTime;
            blackScreenManager.GraceCountdown(true, localTimer);
            yield return null;
        }
        OnWinExecute?.Invoke();
    }


}
