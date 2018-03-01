using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

	//quits the game
	public void quitGame () {
		Application.Quit ();
		Debug.Log ("Quitting Game...");
		//Just for testing in Unity
	}
}
