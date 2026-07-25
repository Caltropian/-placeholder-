using System;
using UnityEngine;

public class AudioRibbitEmitter : MonoBehaviour
{
    [Serializable]
    internal class Range2d
    {
        public float minValue;
        public float maxValue;
        public Range2d(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
    [SerializeField]
    private Range2d ribbitTimeRange;
    [SerializeField]
    public PlayerState.PlayerStates CurrentState;
    [SerializeField]
    float timerToRibbit = 0f;
    [SerializeField]
    private float randomizedValue;
    void OnEnable()
    {
        randomizedValue = UnityEngine.Random.Range(ribbitTimeRange.minValue, ribbitTimeRange.maxValue);
    }

    public void Update()
    {
        if (CurrentState == PlayerState.PlayerStates.UNDERWATER)
        {
            timerToRibbit = 0f;
            return;
        }
        timerToRibbit += Time.deltaTime;
        if (timerToRibbit >= randomizedValue)
        {
            AudioContext.Instance.PlayerAudioEmitter.PlaySfx(PlayerAudioEmitter.PlayerSFXTypes.Ribbit);
            timerToRibbit = 0;
            randomizedValue = UnityEngine.Random.Range(ribbitTimeRange.minValue, ribbitTimeRange.maxValue);
        }

    }
    public void ChangeState(PlayerState.PlayerStates playerStates)
    {
        CurrentState = playerStates;
    }
}
