using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraceGame : MonoBehaviour
{
    public bool EnableBalls = true;
    private bool needAnswerBalls;
    public bool EnableNumbers = false;
    private bool needAnswerNumbers;
    public bool EnableWheel = false;
    private bool needAnswerWheel;
    private TraceWheel wheel;
    public TraceBall BallPrefab;
    private GameObject ballsGameArea;
    private List<GameObject> ballList = new();
    public GameObject ballAnswerPrefab;
    private List<GameObject> answerList = new();
    public int BallCount;
    public float HighlightTargetDuration;
    public float PlayDuration = 60.0f;
    public float ShowResultDuration = 5.0f;
    private Vector3 areaSize;
    private int targetId;
    private GameObject startUi;
    private Button wheelEnterButton;
    private Slider wheelEnterSlider;
    private Button numbersEnterButton;
    private Slider numbersEnterSlider;
    private TextMeshProUGUI resultBalls;
    private TextMeshProUGUI resultNumbers;
    private TextMeshProUGUI resultWheel;
    private TextMeshProUGUI numbersText;
    private TextMeshProUGUI ballsAnwerInstruction;
    public int StartShowNumbers = 0;
    public int IntervalShowNumbers = 4;
    private string numberString;
    private int sumNumber = 0;

    void Start()
    {
        startUi = GameObject.Find("UI");
        ballsGameArea = GameObject.Find("BallsGameArea");
        ballsAnwerInstruction = GameObject.Find("BallsAnwerInstruction").GetComponent<TextMeshProUGUI>();
        resultBalls = GameObject.Find("ResultBallsText").GetComponent<TextMeshProUGUI>();
        resultNumbers = GameObject.Find("ResultNumbersText").GetComponent<TextMeshProUGUI>();
        resultWheel = GameObject.Find("ResultWheelText").GetComponent<TextMeshProUGUI>();

        wheel = GameObject.Find("Wheel").GetComponent<TraceWheel>();
        wheelEnterButton = GameObject.Find("WheelEnterButton").GetComponent<Button>();
        wheelEnterSlider = GameObject.Find("WheelEnterSlider").GetComponent<Slider>();

        numbersText = GameObject.Find("NumbersText").GetComponent<TextMeshProUGUI>();
        numbersEnterButton = GameObject.Find("NumbersEnterButton").GetComponent<Button>();
        numbersEnterSlider = GameObject.Find("NumbersEnterSlider").GetComponent<Slider>();

        wheel.gameObject.SetActive(false);
        wheelEnterButton.gameObject.SetActive(false);
        wheelEnterSlider.gameObject.SetActive(false);
        numbersEnterButton.gameObject.SetActive(false);
        numbersEnterSlider.gameObject.SetActive(false);
        resultBalls.gameObject.SetActive(false);
        resultNumbers.gameObject.SetActive(false);
        resultWheel.gameObject.SetActive(false);
        numbersText.gameObject.SetActive(false);
        ballsAnwerInstruction.gameObject.SetActive(false);
    }

    public void StartTraceGame()
    {
        Debug.Log("Trace game started");
        startUi.gameObject.SetActive(false);

        // Balls
        if (EnableBalls)
        {
            Debug.Log("Balls enabled");

            foreach (TraceBall ball in GetComponentsInChildren<TraceBall>())
                Destroy(ball.gameObject);

            targetId = Random.Range(0, BallCount);
            areaSize = ballsGameArea.GetComponent<SpriteRenderer>().localBounds.size / 2;

            for (int i = 0; i < BallCount; ++i)
                InstantiateGameSpheres(i);
        }

        // Numbers
        if (EnableNumbers)
        {
            Debug.Log("Numbers enabled");

            numberString = "";
            sumNumber = 0;
            numbersEnterSlider.value = 0;
            numbersText.text = numberString;
            numbersText.gameObject.SetActive(true);
            InvokeRepeating("CreateNumbers", StartShowNumbers, IntervalShowNumbers);
        }

        // Wheel
        if (EnableWheel)
        {
            Debug.Log("Wheel enabled");
            wheel.gameObject.SetActive(true);
            wheelEnterSlider.value = 0;
            wheel.StartMove();
        }

        StartCoroutine(EvaluateGame(PlayDuration));
    }

    void InstantiateGameSpheres(int id)
    {
        TraceBall ball = Instantiate(BallPrefab, ballsGameArea.transform);
        ball.SetAreaSize(areaSize);
        ball.Id = id;
        ball.name = "Ball " + id.ToString();
        ballList.Add(ball.gameObject);

        Vector3 position = new Vector3(
            Random.Range(-areaSize.x, areaSize.x),
            Random.Range(-areaSize.y, areaSize.y),
            Random.Range(-areaSize.z, areaSize.z)
        );
        ball.transform.localPosition = position;

        // Highlight target for the first seconds
        if (ball.Id == targetId)
            StartCoroutine(HighlightTarget(ball, HighlightTargetDuration));
    }

    IEnumerator HighlightTarget(TraceBall newUnit, float playDuration)
    {
        Color origColor = newUnit.GetComponent<SpriteRenderer>().material.color;
        newUnit.GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(playDuration);
        newUnit.GetComponent<SpriteRenderer>().material.color = origColor;
    }
    IEnumerator EvaluateGame(float playDuration)
    {
        yield return new WaitForSeconds(playDuration);

        if (EnableBalls)
        {
            needAnswerBalls = true;
            foreach (TraceBall unit in GetComponentsInChildren<TraceBall>())
            {
                unit.ToggleMove = false;
                var idText = unit.GetComponentInChildren<TextMeshPro>();
                idText.text = unit.Id.ToString();
                idText.enabled = true;
            }
            CreateAnswerButtons();
            ballsAnwerInstruction.gameObject.SetActive(true);
        }

        if (EnableNumbers)
        {
            needAnswerNumbers = true;
            CancelInvoke("CreateNumbers"); // Stop number generation
            numbersText.gameObject.SetActive(false); // Hide numbers
            numbersEnterSlider.gameObject.SetActive(true);
            numbersEnterButton.gameObject.SetActive(true);
            numbersEnterButton.GetComponent<Button>().onClick.AddListener(OnNumbersAnswerClick);
        }
        if (EnableWheel)
        {
            needAnswerWheel = true;
            wheel.StopMove();
            wheel.gameObject.SetActive(false);
            wheelEnterButton.gameObject.SetActive(true);
            wheelEnterSlider.gameObject.SetActive(true);
            wheelEnterButton.GetComponent<Button>().onClick.AddListener(OnWheelAnswerClick);
        }
    }

    void CreateAnswerButtons()
    {
        var canvas = GameObject.Find("TraceGameCanvas");
        for (int i = 0; i < BallCount; ++i)
        {
            GameObject answerButton = Instantiate(ballAnswerPrefab, canvas.transform);
            answerButton.transform.localPosition = new Vector3(-200 + i * 75f, -200f, 0f);
            answerButton.name = $"Answer {i}";
            var answerText = answerButton.GetComponentInChildren<TextMeshProUGUI>();
            answerText.text = i.ToString();
            var button = answerButton.GetComponent<Button>();
            int index = i; // create index var,cannot use i here: https://answers.unity.com/questions/1288510/buttononclickaddlistener-how-to-pass-parameter-or.html
            button.onClick.AddListener(delegate { OnBallsAnswerClick(index); });
            answerList.Add(answerButton);
        }
    }
    void OnNumbersAnswerClick()
    {
        string res;
        if (sumNumber == numbersEnterSlider.value)
            res = $"{numbersEnterSlider.value} is the correct sum!";
        else
            res = $"Wrong sum! A total of {sumNumber} was correct!";
        Debug.Log(res);
        resultNumbers.text = res;
        resultNumbers.gameObject.SetActive(true);

        numbersEnterButton.gameObject.SetActive(false);
        numbersEnterSlider.gameObject.SetActive(false);
        needAnswerNumbers = false;
        CheckRestart();
    }
    void OnWheelAnswerClick()
    {
        string res;
        if (wheel.ChangeCount == wheelEnterSlider.value)
            res = $"{wheelEnterSlider.value} is the correct amount of direction changes!";
        else
            res = $"Wrong amount of direction changes! Direction was changed {wheel.ChangeCount} times!";
        Debug.Log(res);
        resultWheel.text = res;
        resultWheel.gameObject.SetActive(true);

        wheelEnterButton.gameObject.SetActive(false);
        wheelEnterSlider.gameObject.SetActive(false);
        needAnswerWheel = false;
        CheckRestart();

    }

    void OnBallsAnswerClick(int index)
    {
        // Delete spheres and create a new array for next game
        foreach (GameObject answer in answerList)
            Destroy(answer);
        answerList = new();
        foreach (GameObject sphere in ballList)
            Destroy(sphere);
        ballList = new();

        ballsAnwerInstruction.gameObject.SetActive(false); // Disable answer hint text

        string res;
        if (index == targetId)
            res = $"{index} is the correct ball!";
        else
            res = $"Wrong Ball! {targetId} was correct!";
        Debug.Log(res);
        resultBalls.text = res;

        resultBalls.gameObject.SetActive(true); // Show result text
        needAnswerBalls = false;
        CheckRestart();
    }

    void CheckRestart()
    {
        // only allow restart if all answers are done
        if (!needAnswerBalls && !needAnswerNumbers && !needAnswerWheel)
            StartCoroutine(AllowRestart(ShowResultDuration));
    }

    IEnumerator AllowRestart(float showResultsDuration)
    {
        yield return new WaitForSeconds(showResultsDuration);
        if (EnableBalls)
        {
            resultBalls.gameObject.SetActive(false);
        }
        if (EnableNumbers)
        {
            resultNumbers.gameObject.SetActive(false);
        }
        if (EnableWheel)
        {
            resultWheel.gameObject.SetActive(false);
        }
        startUi.gameObject.SetActive(true);
    }

    void CreateNumbers()
    {
        int newNumber = Random.Range(-100, 100); // -99 to 99
        while (sumNumber + newNumber < 0 || sumNumber + newNumber > 150)
        {
            newNumber = Random.Range(-100, 100); // draw new if out of range
        }
        sumNumber += newNumber;

        if (numberString != "")
        {
            numberString += newNumber < 0 ? "- " : "+ "; // add sign, starting from second row
        }
        numberString += Mathf.Abs(newNumber).ToString() + "<br>"; // number and newline
        numbersText.text = numberString; // show
    }
    public void SetEnableBalls()
    {
        EnableBalls = EnableBalls ? false : true;
    }
    public void SetEnableNumbers()
    {
        EnableNumbers = EnableNumbers ? false : true;
    }
    public void SetEnableWheel()
    {
        EnableWheel = EnableWheel ? false : true;
    }
}
