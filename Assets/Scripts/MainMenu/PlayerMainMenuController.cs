using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMenuController: MonoBehaviour {

	public GameObject mainMenuPanelPrefab;

	GameObject mainMenuPanel;

	bool showMainMenu = false;

	// Use this for initialization
	void Start () {
		GameObject mainMenuCanvas = GameObject.FindWithTag ("MainMenuCanvas");
		mainMenuPanel = Instantiate (mainMenuPanelPrefab, Vector3.zero, Quaternion.identity, mainMenuCanvas.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			showMainMenu = !showMainMenu;
			openMainMenu(showMainMenu);
		}
	}

	//This function opens or closes the main menu, depending on its current open/closed status
	void openMainMenu(bool show) {
		float height = Camera.main.pixelHeight / 2;
		float width = Camera.main.pixelWidth / 2;

		if (!show)
			width += 1000f;

		mainMenuPanel.transform.position = new Vector3(width, height, 0f);
	}
}
