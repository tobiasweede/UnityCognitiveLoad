using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    Button buttonEEG;
    Button buttonETsimple;
    Button buttonETcomplex;
    Button buttonCL;

    void Start()
    {
        buttonEEG = transform.Find("ButtonEEG").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("EEG"));
        buttonEEG = transform.Find("ButtonETsimple").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("ETsimple"));
        buttonEEG = transform.Find("ButtonETcomplex").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("ETcomplex"));
        buttonEEG = transform.Find("ButtonCL").GetComponent<Button>();
        buttonEEG.onClick.AddListener(() => ChangeScene("CL"));
    }

    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
