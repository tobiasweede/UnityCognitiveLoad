using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthRateGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private Text topInfoLeft;
    private Text topInfoRight;
    private Text topInfoMiddle;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    public int numElements = 20;
    private float minValue = float.MaxValue;
    private float maxValue = float.MinValue;
    private List<float> valueList = new();
    private float yMinimum = 30;
    private float yMaximum = 200f;
    private float xSize = 28f;
    private List<GameObject> observationList = new();
    public Boolean SimulateData = false;
    public float SimulateDataInterval = 1.0f;
    private float simulateDataTimeLeft;

    private void Awake()
    {
        topInfoLeft = transform.Find("Top Info Left").GetComponent<RectTransform>().Find("value").GetComponent<Text>();
        topInfoMiddle = transform.Find("Top Info Middle").GetComponent<RectTransform>().Find("value").GetComponent<Text>();
        topInfoRight = transform.Find("Top Info Right").GetComponent<RectTransform>().Find("value").GetComponent<Text>();
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
    }

    private void Start()
    {
        ShowGraph(numElements, (int _i) => "-" + (_i), (float _f) => "" + Mathf.RoundToInt(_f));
        simulateDataTimeLeft = SimulateDataInterval;
    }

    private void Update()
    {
        if (SimulateData)
        {
            simulateDataTimeLeft -= Time.deltaTime;
            if (simulateDataTimeLeft <0)
            {
                SimulateObservation();
                simulateDataTimeLeft = SimulateDataInterval;
            }
        }
        DestroyObservations();
        ShowObservations();
        SetTopInfo();
    }

    private void SetTopInfo()
    {
        if (valueList.Count == 0) return;
        if (valueList.Min() < minValue)
            topInfoLeft.text = valueList.Min().ToString();
        if (valueList.Max() > maxValue)
            topInfoRight.text = valueList.Max().ToString();
        topInfoMiddle.text = valueList.Average().ToString();

    }
    public void AddObservation(float value)
    {
        if (valueList.Count >= numElements) valueList.RemoveAt(numElements - 1);
        valueList.Insert(0, value);
    }

    private void SimulateObservation()
    {
        if (valueList.Count >= numElements) valueList.RemoveAt(numElements - 1);
        valueList.Insert(0, UnityEngine.Random.Range(30f, 200f));
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(11, 11);
        return gameObject;
    }
    private void DestroyObservations()
    {
        foreach (GameObject item in observationList)
        {
            Destroy(item);
        }
    }

    private float getXPosition(int index, float xSize)
    {
        return index * xSize - xSize / 2;
    }

    private void ShowObservations()
    {
        float graphHeight = graphContainer.sizeDelta.y;
        GameObject lastDotGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = getXPosition(i + 1, xSize);
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum + yMinimum)) * graphHeight;
            GameObject dotGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            observationList.Add(dotGameObject);
            if (lastDotGameObject != null)
            {
                CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastDotGameObject = dotGameObject;
        }

    }

    private void ShowGraph(int numElements, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        float graphHeight = graphContainer.sizeDelta.y;

        for (int i = 1; i <= numElements; i++)
        {
            float xPosition = getXPosition(i, xSize);
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -4f);
        }

        int seperatorCount = 10;
        for (int i = 1; i <= seperatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / seperatorCount;
            labelY.anchoredPosition = new Vector2(-25f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY((normalizedValue * (yMaximum - yMinimum)) + yMinimum);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        observationList.Add(gameObject);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f); // white with 0.5 alpha 
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }
}
