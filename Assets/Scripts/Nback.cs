using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Nback : MonoBehaviour
{
    public int loopTime = 5;
    public int minRange = 1;
    public int maxRange = 3;
    public int sequenceLength = 10;
    private int currentIndex;
    private List<int> sequence;
    private TextMeshProUGUI targetText;
    private TextMeshProUGUI responseText;
    private GameObject startDialog;
    private GameObject nbackDialog;
    private Button startButton;
    private Button responseButton;
    private int n;
    private Vector2 targetStartPosition;


    void Start()
    {
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        responseButton = GameObject.Find("ResponseButton").GetComponent<Button>();
        startDialog = GameObject.Find("StartDialog");
        nbackDialog = GameObject.Find("NbackDialog");
        targetText = GameObject.Find("TargetText").GetComponent<TextMeshProUGUI>();
        responseText = GameObject.Find("ResponseText").GetComponent<TextMeshProUGUI>();

        targetStartPosition = targetText.transform.position;
        startButton.onClick.AddListener(StartNback);
        responseButton.onClick.AddListener(CheckResponse);

        nbackDialog.SetActive(false); // not before any find in this hierarchy!
    }

    void Update()
    {
        // targetText.transform.position += targetText.transform.right * 50f * Time.deltaTime;
        targetText.transform.localScale = targetText.transform.localScale + Vector3.one * 1.0f * Time.deltaTime;
    }

    void InitializeSequence()
    {
        sequence = new List<int>();

        // Generate a random sequence of numbers (for simplicity, let's use integers)
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomValue = Random.Range(minRange, maxRange + 1); // Adjust the range as needed
            sequence.Add(randomValue);
        }
    }
    void CheckResponse()
    {
        if (currentIndex < n + 1)
        {
            responseText.text = "Sequence too short. Nothing to do.";
            return;
        }
        if (sequence[currentIndex] == sequence[currentIndex - n])
        {
            responseText.text = "Correct";
        }
        else
        {
            responseText.text = "Wrong";
        }
    }

    void StartNback()
    {
        n = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
        startDialog.SetActive(false);
        nbackDialog.SetActive(true);
        responseText.text = "";
        InitializeSequence();
        InvokeRepeating("NbackLoop", 0, loopTime);
    }
    void NbackLoop()
    {
        responseText.text = string.Join(",", sequence.ToArray());

        targetText.transform.position = targetStartPosition;
        targetText.transform.localScale = Vector3.one;
        targetText.text = sequence[currentIndex++].ToString();
    }
}
