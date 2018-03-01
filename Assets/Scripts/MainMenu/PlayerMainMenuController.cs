using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMenuController: MonoBehaviour {

	public GameObject mainMenuPanelPrefab;
	GameObject mainMenuPanel;
	bool showMainMenu = false;
	PlayerController playerController;
	AnimationBehaviour playerAnimation;

	// Use this for initialization
	void Start () {
		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		mainMenuPanel = Instantiate (mainMenuPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
		mainMenuPanel.SetActive (false);

		playerController = GetComponent<PlayerController> ();
		playerAnimation = GetComponent<AnimationBehaviour> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			toggleMainMenu();
		}
	}

	//This function opens or closes the main menu, depending on its current open/closed status
	public void toggleMainMenu() {
		showMainMenu = !showMainMenu;

		mainMenuPanel.SetActive (showMainMenu);
		playerController.enabled = !showMainMenu;
		playerAnimation.enabled = !showMainMenu;
	}
}
