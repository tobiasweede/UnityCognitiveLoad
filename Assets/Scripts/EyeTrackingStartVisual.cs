using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EyeTrackingStartVisual : MonoBehaviour
{
    [SerializeField] float playDuration = 40f;
    LslOutlet lslOutlet;
    Text title;
    Text eval;
    List<GameObject> productList = new();
    [SerializeField] int productCount = 4;
    Button startButton;
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartVisual);

        lslOutlet = GetComponent<LslOutlet>();

        title = GameObject.Find("Title").GetComponent<Text>();

        eval = GameObject.Find("Eval").GetComponent<Text>();
        eval.gameObject.SetActive(false);

        for (int i = 0; i < productCount; i++)
        {
            string productName = $"Product {i + 1}";
            GameObject product = GameObject.Find(productName);
            productList.Add(product);
            product.SetActive(false);
        }
    }

    // Update is called once per frame
    void StartVisual()
    {
        startButton.interactable = false;
        eval.gameObject.SetActive(false);
        lslOutlet.Send($"Starting {title.text}");
        foreach (GameObject product in productList)
        {
            product.SetActive(true);
            EyeTrackingEventVisual eyeTrackingEventVisual = product.GetComponent<EyeTrackingEventVisual>();
            eyeTrackingEventVisual.ResetVisual();

        }
        StartCoroutine(EvaluateVisual(playDuration));
    }

    IEnumerator EvaluateVisual(float playDuration)
    {
        yield return new WaitForSeconds(playDuration);

        lslOutlet.Send($"Stopping {title.text}");
        foreach (GameObject product in productList)
            product.SetActive(false);
        eval.gameObject.SetActive(true);
        startButton.interactable = true;
    }
}
