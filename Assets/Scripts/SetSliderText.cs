using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetSliderText : MonoBehaviour
{
    private TextMeshProUGUI sliderText;

    void Start()
    {
        sliderText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { SetText(); });
    }

    public void SetText()
    {
        sliderText.GetComponent<TextMeshProUGUI>().text = GetComponent<Slider>().value.ToString();
    }
}
