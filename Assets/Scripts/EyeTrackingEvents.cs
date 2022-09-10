using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackingEvents : MonoBehaviour
{
    public enum EyeTrackingEventType
    {
        Blink,
        Fixation,
        Saccade,
    }
    public string[] ProductArray = {
        "Product 1",
        "Product 2",
        "Product 3",
        "Product 4",
    };
    private int lastProductIndex;
    public class EyeTrackingEvent
    {
        public EyeTrackingEventType type { get; }
        public DateTime start { get; }
        public float duration { get; }
        public float velocity { get; }
        public string product { get; }

        public EyeTrackingEvent(EyeTrackingEventType type, DateTime start, float duration, string product)
        {
            this.type = type;
            this.start = start;
            this.duration = duration;
            this.product = product;
        }
    }
    public Action<EyeTrackingEvent> eyeTrackingEventCallback;

    private float timer;

    void Start()
    {
        timer = RandomExponentialVariable();
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = RandomExponentialVariable();
                var duration = RandomGaussian(0.01f, timer);
                EyeTrackingEventType eventType = RandomEnumValue<EyeTrackingEventType>();
                int productIndex = chooseProductIndex();
                string product = ProductArray[productIndex];
                EyeTrackingEvent eyeTrackingEvent = new EyeTrackingEvent(eventType, DateTime.Now, duration, product);
                eyeTrackingEventCallback(eyeTrackingEvent);
            }
        }
    }

    private int chooseProductIndex(float retainProb = 0.7f)
    {
        if (UnityEngine.Random.value < retainProb)
            return lastProductIndex;

        int productIndex = UnityEngine.Random.Range(0, ProductArray.Length);
        lastProductIndex = productIndex;
        return productIndex;
    }

    // https://answers.unity.com/questions/421968/normal-distribution-random.html
    // https://stackoverflow.com/questions/75677/converting-a-uniform-distribution-to-a-normal-distribution
    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }

    // https://en.wikipedia.org/wiki/Exponential_distribution#Random_variate_generation
    public static float RandomExponentialVariable(float lambda = 3.0f, float minValue = 0.1f, float maxValue = 3.0f)
    {
        float time = (float)(-1 * Math.Log(UnityEngine.Random.value) / lambda);
        return Math.Clamp(time, minValue, maxValue);
    }

    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }
}
