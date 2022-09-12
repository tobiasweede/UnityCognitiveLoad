using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeTrackingEventVisual : MonoBehaviour
{
    [SerializeField] bool ComplexVisual = false;
    TextMeshProUGUI title;
    TextMeshProUGUI fixationCountText;
    TextMeshProUGUI fixationMeanDurationText;
    TextMeshProUGUI fixationLastDurationText;
    TextMeshProUGUI dilationMeanText;
    TextMeshProUGUI dilationLastText;
    TextMeshProUGUI blinkCountText;
    TextMeshProUGUI blinkMeanDurationText;
    int fixationCount { get; set; }
    float fixationSumDuration { get; set; }
    int dilationCount { get; set; }
    float dilationSum { get; set; }
    int blinkCount { get; set; }
    float blinkSumDuration { get; set; }

    void Start()
    {
        EyeTrackingEvents eyeTrackingEvents = GetComponent<EyeTrackingEvents>();

        // Set title according to gameObject name
        title = transform.Find("title").GetComponent<TextMeshProUGUI>();
        title.text = "<b>" + gameObject.name + "</b>";

        // Find dashboard elements
        fixationCountText = transform.Find("fixationCountText").GetComponent<TextMeshProUGUI>();
        fixationMeanDurationText = transform.Find("fixationMeanDurationText").GetComponent<TextMeshProUGUI>();
        fixationLastDurationText = transform.Find("fixationLastDurationText").GetComponent<TextMeshProUGUI>();
        dilationMeanText = transform.Find("dilationMeanText").GetComponent<TextMeshProUGUI>();
        dilationLastText = transform.Find("dilationLastText").GetComponent<TextMeshProUGUI>();
        blinkCountText = transform.Find("blinkCountText").GetComponent<TextMeshProUGUI>();
        blinkMeanDurationText = transform.Find("blinkMeanDurationText").GetComponent<TextMeshProUGUI>();

        // Set event callback function
        if (ComplexVisual)
            eyeTrackingEvents.eyeTrackingEventCallback = VisualizeComplex;
        else
            eyeTrackingEvents.eyeTrackingEventCallback = VisualizeSimple;
    }
    private void VisualizeComplex(EyeTrackingEvents.EyeTrackingEvent eyeTrackingEvent)
    {
        if (!gameObject.name.Equals(eyeTrackingEvent.product))
            return; // Not this product

        switch (eyeTrackingEvent.type)
        {
            case EyeTrackingEvents.EyeTrackingEventType.Fixation:
                fixationSumDuration += eyeTrackingEvent.duration;
                fixationCount++;
                fixationCountText.text = fixationCount.ToString();
                fixationMeanDurationText.text = Math.Round((fixationSumDuration / fixationCount), 2).ToString() + " ms";
                fixationLastDurationText.text = eyeTrackingEvent.duration.ToString();
                break;

            case EyeTrackingEvents.EyeTrackingEventType.Blink:
                blinkSumDuration += eyeTrackingEvent.duration;
                blinkCount++;
                blinkCountText.text = blinkCount.ToString();
                blinkMeanDurationText.text = Math.Round((blinkSumDuration / blinkCount), 2).ToString() + " ms";
                break;
        }

        dilationCount++;
        dilationSum += eyeTrackingEvent.dilation;
        dilationLastText.text = Math.Round(eyeTrackingEvent.dilation, 2).ToString();
        dilationMeanText.text = Math.Round((dilationSum / dilationCount), 2).ToString() + " ms";
    }

    private void VisualizeSimple(EyeTrackingEvents.EyeTrackingEvent eyeTrackingEvent)
    {
        if (!gameObject.name.Equals(eyeTrackingEvent.product))
            return;

        if (eyeTrackingEvent.type != EyeTrackingEvents.EyeTrackingEventType.Fixation)
            return;

        fixationSumDuration += eyeTrackingEvent.duration;
        fixationCount += 1;
        fixationCountText.text = fixationCount.ToString();
        fixationMeanDurationText.text = Math.Round((fixationSumDuration / fixationCount), 2) + " ms";
    }

    public void ResetVisual()
    {
        fixationCount = 0;
        fixationSumDuration = 0;
        blinkCount = 0;
        fixationCount = 0;
        dilationCount = 0;
        dilationSum = 0;
    }
}