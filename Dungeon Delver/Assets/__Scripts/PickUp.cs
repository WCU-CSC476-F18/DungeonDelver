using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public enum eType { key, health, grappler, treasure }

    public static float COLLIDER_DELAY = 0.5f;

    [Header("Set in Inspector")]
    public eType itemType;

    //Awake() and Activate() disable the PickUp's Collider for 0.5 secs
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("Activate", COLLIDER_DELAY);
    }

    void Activate()
    {
        GetComponent<Collider>().enabled = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
