using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelMap : MonoBehaviour {

	Vector3 forestLocation { get { return new Vector3 (244f, 141f, Camera.main.transform.localScale.z); } }

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
			Camera.main.transform.position = forestLocation;
			GameObject player = GameManager.instance.LocalPlayer();
			player.transform.position = forestLocation;
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
