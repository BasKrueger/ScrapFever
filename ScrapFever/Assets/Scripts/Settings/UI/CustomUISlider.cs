using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CustomUISlider : MonoBehaviour
{
    public event Action<float> ValueChanged;

    public float value
    {
        get
        {
            return Mathf.Clamp(sl.value, 0.001f, 1f);
        }
        set
        {
            if (sl.value == value) return;
            sl.value = value;
            valueChanged(sl.value);
        }
    }

    [SerializeField]
    private Slider sl;
    [SerializeField]
    private TextMeshProUGUI text;

    private void Awake()
    {
        sl.onValueChanged.AddListener(valueChanged);
    }

    private void valueChanged(float value)
    {
        text.text = Mathf.RoundToInt(sl.value * 100).ToString();
        ValueChanged?.Invoke(Mathf.Clamp(sl.value, 0.001f, 1f));
    }
}
