using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Nback : MonoBehaviour
{
    public int loopTime = 5;
    public int minRange = 1;
    public int maxRange = 3;
    private int currentIndex;
    private List<int> sequence;
    private TextMeshProUGUI targetText;
    private TextMeshProUGUI responseText;
    private TextMeshProUGUI sequenceText;
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
        sequenceText = GameObject.Find("SequenceText").GetComponent<TextMeshProUGUI>();

        targetStartPosition = targetText.transform.position;
        startButton.onClick.AddListener(StartNback);
        responseButton.onClick.AddListener(CheckResponse);

        nbackDialog.SetActive(false); // not before any find in this hierarchy!
    }

    void Update()
    {
        moveText();
    }

    void InitializeSequence()
    {
        sequence = new List<int>();

        // Generate a random sequence of numbers (for simplicity, let's use integers)
        for (int i = 0; i < n; i++)
        {
            int randomValue = Random.Range(minRange, maxRange + 1); // Adjust the range as needed
            sequence.Add(randomValue);
        }
        Debug.Log(sequence);
    }
    void CheckResponse()
    {
        if (sequence.Last() == sequence.ElementAtOrDefault(sequence.Count - n - 1))
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
        StartCoroutine("NbackLoop");
    }
    IEnumerator NbackLoop()
    {
        while (true)
        {
            // Add new random variable and rotate list            
            int randomValue = Random.Range(minRange, maxRange + 1);
            sequence.Add(randomValue);
            // sequence.RemoveAt(0);

            sequenceText.text = string.Join(", ", sequence.ToArray());

            targetText.transform.position = targetStartPosition;
            targetText.transform.localScale = Vector3.one;
            targetText.text = sequence.Last().ToString();

            yield return new WaitForSeconds(loopTime);
        }
    }

    void moveText()
    {
        // targetText.transform.position += targetText.transform.right * 50f * Time.deltaTime;
        targetText.transform.localScale = targetText.transform.localScale + Vector3.one * 1.0f * Time.deltaTime;
    }
}
