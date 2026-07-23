using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class OxygenSliderBehaviour : MonoBehaviour
{
    [Tooltip("Match with Slider MaxValue")]
    [SerializeField]
    private float _maxValue = 1;
    [Header("Color Settings")]
    [SerializeField]
    private Color _startingColor;
    [SerializeField]
    private Color _endingColor;
    private float _currValue;


    public float MaxValue { get; set; }
    [Header("Dependencies")]
    [SerializeField]
    private RectTransform rectTransformTarget;
    private Image imageTarget;

    void OnEnable()
    {
        _currValue = _maxValue;
    }
    void Start()
    {
        imageTarget = rectTransformTarget.GetComponent<Image>();
    }

    public void ChangeValue(float newValue)
    {
        _currValue = newValue / _maxValue;
        rectTransformTarget.localScale = new(newValue / _maxValue, newValue / _maxValue, 1);
        imageTarget.color = Color.Lerp(_endingColor, _startingColor, _currValue / _maxValue);
    }
}
