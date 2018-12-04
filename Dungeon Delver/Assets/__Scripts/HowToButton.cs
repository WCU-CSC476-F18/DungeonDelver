using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToButton : MonoBehaviour {

    public Button howToButton;
    public Text Controls;
    public GameObject grapplerPickup;
	// Use this for initialization
	void Start () {
        howToButton.GetComponent<Button>();
        howToButton.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void TaskOnClick()
    {
        Controls.GetComponent<Text>();
        Controls.text = "Move: W, A, S, and D keys" +
            "\nSword: SPACEBAR\nGrappler: J" +
            "\nTraverse a dangerous dungeon to find the treasure at the end";
        grapplerPickup.GetComponent<GameObject>();
        Vector3 grapplerPosition = new Vector3(-4.43f, -1.91f, 0);
        grapplerPickup.transform.localScale = new Vector3(1, 1, 1);
        Instantiate(grapplerPickup, grapplerPosition, transform.rotation);
    }
}
