using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscHandler : MonoBehaviour
{
    string scene;
    void Start()
    {
        scene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
        if (scene == "Menu")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
       } 
    }
}
