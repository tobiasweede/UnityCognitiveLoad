using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    private Button buttonMultitasking;
    private Button buttonNback;
    private Button buttonDashboard;

    void Start()
    {
        buttonMultitasking = transform.Find("ButtonMultitasking").GetComponent<Button>();
        buttonMultitasking.onClick.AddListener(() => ChangeScene("Multitasking"));
        buttonNback = transform.Find("buttonNback").GetComponent<Button>();
        buttonNback.onClick.AddListener(() => ChangeScene("Nback"));
        buttonDashboard = transform.Find("ButtonDashboard").GetComponent<Button>();
        buttonDashboard.onClick.AddListener(() => ChangeScene("Dashboard"));
    }

    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
