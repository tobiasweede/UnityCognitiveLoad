using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeTrackingEventVisual : MonoBehaviour
{
    [SerializeField] bool ComplexVisual = false;
    TextMeshProUGUI fixationCountText;
    TextMeshProUGUI fixationMeanDurationText;
    TextMeshProUGUI title;
    int fixationCount { get; set; }
    float sumDuration { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        EyeTrackingEvents eyeTrackingEvents = GetComponent<EyeTrackingEvents>();
        title = transform.Find("title").GetComponent<TextMeshProUGUI>();
        title.text = "<b>" + gameObject.name + "</b>";
        fixationCountText = transform.Find("fixationCountText").GetComponent<TextMeshProUGUI>();
        fixationMeanDurationText = transform.Find("fixationMeanDurationText").GetComponent<TextMeshProUGUI>();
        if (ComplexVisual)
            eyeTrackingEvents.eyeTrackingEventCallback = VisualizeComplex;
        else
            eyeTrackingEvents.eyeTrackingEventCallback = VisualizeSimple;
    }
    private void VisualizeComplex(EyeTrackingEvents.EyeTrackingEvent eyeTrackingEvent)
    {
        if (!gameObject.name.Equals(eyeTrackingEvent.product))
            return;

        if (eyeTrackingEvent.type != EyeTrackingEvents.EyeTrackingEventType.Fixation)
            return;

        sumDuration += eyeTrackingEvent.duration;
        fixationCount += 1;
        fixationCountText.text = fixationCount.ToString();
        fixationMeanDurationText.text = (sumDuration / fixationCount).ToString();
    }

    private void VisualizeSimple(EyeTrackingEvents.EyeTrackingEvent eyeTrackingEvent)
    {
        if (!gameObject.name.Equals(eyeTrackingEvent.product))
            return;

        if (eyeTrackingEvent.type != EyeTrackingEvents.EyeTrackingEventType.Fixation)
            return;

        sumDuration += eyeTrackingEvent.duration;
        fixationCount += 1;
        fixationCountText.text = fixationCount.ToString();
        fixationMeanDurationText.text = Math.Round((sumDuration / fixationCount), 2) + " ms";
    }

    public void ResetVisual()
    {
        fixationCount = 0;
        sumDuration = 0;
    }
}
