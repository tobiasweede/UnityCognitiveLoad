using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    private Button buttonEEG;
    private Button buttonCL;
    // Button buttonETsimple;
    // Button buttonETcomplex;

    void Start()
    {
        buttonEEG = transform.Find("ButtonMultitasking").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("Multitasking"));
        buttonEEG = transform.Find("ButtonDashboard").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("Dashboard"));

        // buttonEEG = transform.Find("ButtonETsimple").GetComponent<Button>();
        // buttonEEG.onClick.AddListener(() => ChangeScene("ETsimple"));
        // buttonEEG = transform.Find("ButtonETcomplex").GetComponent<Button>();
        // buttonEEG.onClick.AddListener(() => ChangeScene("ETcomplex"));
    }

    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
