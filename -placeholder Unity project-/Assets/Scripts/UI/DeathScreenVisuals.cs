using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenVisuals : IPausable
{
    [Header("Timers")]
    [SerializeField]
    private float timerWaitUntilRespawn = 1.0f;
    [SerializeField]
    private float timerGraceRecover = 0.5f;
    [SerializeField]
    private float timerRespawnRecover = 1.0f;
    [Header("Dependencies")]
    [SerializeField]
    private Image blackScreenImage;

    public static event Action OnScreenClear;
    public static event Action OnScreenBlack;
    private float maxTimer = 0f;
    private float currAlpha = 0f;
    private bool isFirstTimeInGrace = true;
    private IEnumerator timerEnumerator;
    public bool HasWon = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        OxygenTracker.GracePeriodValueChange += GraceCountdown;
        OxygenTracker.OnDrown += DeathRespawnCooldown;

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (!HasWon)
        {
            OxygenTracker.GracePeriodValueChange -= GraceCountdown;
            OxygenTracker.OnDrown -= DeathRespawnCooldown;
        }

    }
    public void ActivateWon()
    {
        HasWon = true;
        OxygenTracker.GracePeriodValueChange -= GraceCountdown;
        OxygenTracker.GracePeriodValueChange -= GraceCountdown;
    }
    public void GraceCountdown(bool isGracing, float timeUntilDeath)
    {
        if (isGamePaused) return;
        if (isGracing)
        {
            if (isFirstTimeInGrace)
            {
                maxTimer = timeUntilDeath;
            }
            isFirstTimeInGrace = false;
            Color newScreenColor = blackScreenImage.color;
            newScreenColor.a = Mathf.Lerp(1, 0, timeUntilDeath / maxTimer);
            blackScreenImage.color = newScreenColor;
            if (blackScreenImage.color.a == 1)
            {
                OnScreenBlack?.Invoke();
            }
        }
        else
        {
            if (timerEnumerator != null)
            {
                StopCoroutine(timerEnumerator);
            }
            isFirstTimeInGrace = true;
            currAlpha = blackScreenImage.color.a;
            timerEnumerator = CountdownEnumerator(countdownTime =>
            {
                //Optimization: base operation on color lerp instead of doing a math lerp on the alpha.
                Color newScreenColor = blackScreenImage.color;
                newScreenColor.a = Mathf.Lerp(0, currAlpha, countdownTime / (timerGraceRecover * currAlpha));
                blackScreenImage.color = newScreenColor;
            }, timerGraceRecover * currAlpha);
            StartCoroutine(timerEnumerator);
        }
    }
    private void DeathRespawnCooldown()
    {
        //Make sure it has an alpha of 1.
        Color newScreenColor = blackScreenImage.color;
        newScreenColor.a = 1;
        blackScreenImage.color = newScreenColor;
        if (timerEnumerator != null)
        {
            StopCoroutine(timerEnumerator);
        }
        timerEnumerator = CountdownEnumerator(countdownTimer =>
        {
            if (countdownTimer > timerRespawnRecover)
            {
                return;
            }
            else
            {
                //fade back in
                Color newScreenColor = blackScreenImage.color;
                newScreenColor.a = Mathf.Lerp(0, 1, countdownTimer / timerRespawnRecover);
                blackScreenImage.color = newScreenColor;
            }
        }, timerWaitUntilRespawn + timerRespawnRecover,
        _ =>
        {
            OnScreenClear?.Invoke();
        });
        StartCoroutine(timerEnumerator);
    }
    private IEnumerator CountdownEnumerator(Action<float> callback, float maxTimer, Action<float> lastCallback = null)
    {
        float currTimer = maxTimer;
        while (currTimer > 0)
        {
            if (!isGamePaused)
            {
                currTimer -= Time.deltaTime;
                callback(currTimer);
                yield return null;
            }
        }
        lastCallback?.Invoke(0);
    }
}
