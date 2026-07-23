using TMPro;
using UnityEngine;

public class RoughBreathCounter : MonoBehaviour
{
    public RoughBreathScript breath;
    TMP_Text meter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meter = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        meter.text = breath.currentBreath.ToString();
    }
}
