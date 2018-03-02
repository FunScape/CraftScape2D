using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (gameObject.name == "GoToForestScene")
        {
            Application.LoadLevel("ForestScene");
        }
        else if (gameObject.name == "GoToRiverScene")
        {
            Application.LoadLevel("RiverScene");
        }
        else if (gameObject.name == "GoToMainScene")
        {
            Application.LoadLevel("MainScene");
        }

    }
}
