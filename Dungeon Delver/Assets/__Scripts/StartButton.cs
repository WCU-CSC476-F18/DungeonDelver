using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public Button startButton;
    // Use this for initialization
    void Start()
    {
        startButton.GetComponent<Button>();
        startButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("_Scene_Hat");
    }
}