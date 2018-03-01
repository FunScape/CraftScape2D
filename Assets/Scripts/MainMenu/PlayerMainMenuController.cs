using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMainMenuController: MonoBehaviour {

	public GameObject mainMenuPanelPrefab;
	GameObject mainMenuPanel;

	bool showMainMenu = false;
	PlayerController playerController;
	AnimationBehaviour playerAnimation;

	float cameraHeight;
	float cameraWidth;

	// Use this for initialization
	void Start () {
		cameraHeight = Camera.main.pixelHeight;
		cameraWidth = Camera.main.pixelWidth;

		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		mainMenuPanel = Instantiate (mainMenuPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
		mainMenuPanel.transform.position = new Vector3(cameraWidth * 2, cameraHeight / 2, 0f);

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

		if (showMainMenu)
			mainMenuPanel.transform.position = new Vector3(cameraWidth / 2, cameraHeight / 2, 0f);
		else
			mainMenuPanel.transform.position = new Vector3(cameraWidth * 2, cameraHeight / 2, 0f);

		//mainMenuPanel.SetActive (showMainMenu);
		playerController.enabled = !showMainMenu;
		playerAnimation.enabled = !showMainMenu;
	}
}
