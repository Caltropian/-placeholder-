using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;

public class OxygenTracker : IPausable
{
    [Serializable]
    internal class Range2
    {
        public float minValue; public float maxValue; public bool graceExists;
        public Range2(float minValue, float maxValue, bool graceExists = true)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.graceExists = graceExists;
        }
    }
    #region SerializableParameters
    [Header("Adjustable Settings")]
    [Tooltip("How much time (in seconds) will it take to completely run out of oxygen")]
    [SerializeField]
    private float timeToDepleteOxygen = 30f;
    [Tooltip("How much time (in seconds) will it take to refill the oxygen bar in an air pocket")]
    private float timeToRefillOxygen = 2f;
    [Tooltip("How much time (in seconds) will the grace period, before death, last. (0 for no Grace Period)")]
    [SerializeField]
    private Range2 gracePeriodBeforeDeath = new(1f, 2f);
    [Tooltip("How much time (in seconds) will be added on collision with an air bubble")]
    [SerializeField]
    private float airBubbleTime = 10f;

    [Header("Adjustable Settings")]
    [SerializeField]
    private bool visualizeGracePeriod = true;
    [SerializeField]
    private bool canDie = true;

    [Header("Events")]
    public UnityEvent<float> OnValueChanged;
    #endregion

    #region Local Parameters
    private readonly float _maxOxygen = 1;
    private bool _isUnderwater = false;
    private bool _isOnGracePeriod = false;
    private bool _ranOutOfOxygen = false;
    private float _currentOxygen;
    private float _currentGraceTimer = 0.0f;
    #endregion

    /// <summary>
    /// Whenever the player is about to die, there's a small grace period. Use this for blackout-like effects.
    /// First Parameters: IsGracePeriodActive? Second Parameter: Value of Grace Period. 
    /// </summary>
    public static event Action<bool, float> GracePeriodValueChange;
    public static event Action OnDrown;

    public float CurrentOxygen
    {
        protected set
        {
            _currentOxygen = value;
            if ((_isOnGracePeriod && visualizeGracePeriod) || !_isOnGracePeriod)
            {
                OnValueChanged?.Invoke(_currentOxygen);
            }
        }
        get
        {
            return _currentOxygen;
        }
    }
    void Awake()
    {
        _currentOxygen = _maxOxygen;
    }
    void Update()
    {
        if (isGamePaused) return;
        if (_ranOutOfOxygen) return;
        if (!_isUnderwater && (_currentOxygen == _maxOxygen)) return;
        if (_isUnderwater)
        {
            CurrentOxygen -= Time.deltaTime / (_isOnGracePeriod ? _currentGraceTimer : timeToDepleteOxygen);
            //Only Invoke when tracking a grace period.
            if (_isOnGracePeriod)
            {
                if (canDie) GracePeriodValueChange?.Invoke(_isOnGracePeriod, CurrentOxygen);
            }
            if (_currentOxygen <= 0)
            {
                if (gracePeriodBeforeDeath.graceExists && !_isOnGracePeriod)
                {
                    _isOnGracePeriod = true;
                    _currentGraceTimer = UnityEngine.Random.Range(gracePeriodBeforeDeath.minValue, gracePeriodBeforeDeath.maxValue);
                    CurrentOxygen = _maxOxygen;
                }
                else
                {
                    if (canDie)
                    {
                        _ranOutOfOxygen = true;
                        OnDrown?.Invoke();
                    }
                    else
                    {
                        _isOnGracePeriod = false;
                    }
                }
            }
        }
        else
        {
            CurrentOxygen += Time.deltaTime / timeToRefillOxygen;
            //Only Invoke once after the grace period is reverted.
            if (_isOnGracePeriod)
            {
                GracePeriodValueChange?.Invoke(false, CurrentOxygen);
            }
            _isOnGracePeriod = false;
            if (_currentOxygen >= _maxOxygen)
            {
                _currentOxygen = _maxOxygen;
            }
        }

    }
    public void OnChangedPlayerState(PlayerState.PlayerStates state)
    {
        _isUnderwater = state switch
        {
            PlayerState.PlayerStates.UNDERWATER => true,
            _ => false,
        };
    }
    public void ResetState()
    {
        _currentOxygen = _maxOxygen;
        _isOnGracePeriod = false;
        _ranOutOfOxygen = false;
    }

    public void OnBubbleCollide()
    {
        if (_isOnGracePeriod)
        {
            _isOnGracePeriod = false;
        }
        CurrentOxygen = Mathf.Clamp((airBubbleTime / timeToDepleteOxygen) + CurrentOxygen, 0, _maxOxygen);
    }

}
