using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(Text))]
public class SetOnSliderValue : MonoBehaviour {
    private Text ValueText;

    private void Start()
    {
        ValueText = GetComponent<Text>();
    }

    public void OnSliderValueChanged(float value)
    {
        ValueText.text = value.ToString("0");
    }
}
