/*
 * APinchSensitivity  - 双指挤压的敏感度 ，驱动MainCamera的 AroundAlignCamera.PinchSensitivity
 * ASlideSensitivity  - 双指平移拖拽的敏感度 ，驱动MainCamera的 AroundAlignCamera.SlideSensitivity
 * MTouchpointerSensitivity  - 双指挤压的敏感度 ，驱动Traget的 MouseTranslate.TouchpointerSensitivity
 */

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchSettingHandler : MonoBehaviour
{
    [SerializeField] InputField InputField;
    [SerializeField] Slider slider;
    [SerializeField] Text text;
    [SerializeField] string PlayerPrefsKey;
    [SerializeField] OnValueChanged OnSliderValueChanged = new OnValueChanged();

    [Serializable] public class OnValueChanged : UnityEvent<float> { }

    void Start()
    {
        if (!string.IsNullOrEmpty(PlayerPrefsKey))
        {
            text.text = PlayerPrefs.GetFloat(PlayerPrefsKey).ToString();
        }
        InputField.onEndEdit.AddListener(v =>
        {
            SendMessage();
        });

        slider.onValueChanged.AddListener(v =>
        {
            SendMessage();
        });

        gameObject.SetActive(false);
    }

    void SendMessage()
    {
        float factor = 1;
        if (!string.IsNullOrEmpty(InputField.text))
        {
            float.TryParse(InputField.text, out factor);
        }
        text.text = (factor * slider.value).ToString();
        OnSliderValueChanged.Invoke(factor * slider.value);
    }

}
