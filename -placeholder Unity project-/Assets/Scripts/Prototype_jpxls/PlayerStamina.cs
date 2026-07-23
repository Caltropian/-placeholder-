using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField]
    int noMaxStamina = 5;

    [SerializeField]
    float regenTimePerStaminaPoint = 0.5f;
    [SerializeField]
    float regenTimePerStaminaPointWhenOverloaded = 0.25f;

    [SerializeField]
    Slider staminaVisuals;

    private int _currStamina;
    private Image fillImage;
    public int CurrStamina
    {
        get
        {
            return _currStamina;
        }
        protected set
        {
            _currStamina = value;
            staminaVisuals.value = _currStamina;
        }
    }
    private float regenLocalTimer;
    private float regenLocalMax;
    private bool _isOverloaded = false;

    public bool IsOverloaded
    {
        get { return _isOverloaded; }
        set
        {
            if (value && value != _isOverloaded)
            {
                ChangeFillAlpha(0.5f);
                regenLocalMax = regenTimePerStaminaPointWhenOverloaded;
            }
            else if (!value && value != _isOverloaded)
            {
                ChangeFillAlpha(1f);
                regenLocalMax = regenTimePerStaminaPoint;
            }
            _isOverloaded = value;

        }
    }

    void OnEnable()
    {
        _currStamina = noMaxStamina;
        regenLocalTimer = 0.0f;
        staminaVisuals.value = _currStamina;
        staminaVisuals.maxValue = noMaxStamina;
        fillImage = staminaVisuals.fillRect.GetComponent<Image>();
        regenLocalMax = regenTimePerStaminaPoint;
    }
    public void UseStamina()
    {
        if (CurrStamina == 0) return;
        CurrStamina--;
        regenLocalTimer = 0;
        if (CurrStamina == 0)
        {
            IsOverloaded = true;
        }
    }
    void Update()
    {
        if (regenLocalTimer >= regenLocalMax)
        {
            regenLocalTimer = 0;
            CurrStamina++;
            if (_currStamina == noMaxStamina)
            {
                IsOverloaded = false;
            }
        }
        if (CurrStamina < noMaxStamina)
        {
            regenLocalTimer += Time.deltaTime;
        }
    }
    public bool CanDash()
    {
        return _currStamina > 0 && !IsOverloaded;
    }
    private void ChangeFillAlpha(float newAlpha)
    {
        Color oldColor = fillImage.color;
        oldColor.a = newAlpha;
        fillImage.color = oldColor;
    }
}
