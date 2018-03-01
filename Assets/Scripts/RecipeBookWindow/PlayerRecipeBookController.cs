using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	GameObject recipeBookPanel;

	public RecipeBook recipeBook;

	bool showRecipeBook = false;

	float cameraHeight;
	float cameraWidth;

	// Use this for initialization
	void Start () {
		
		cameraHeight = Camera.main.pixelHeight;
		cameraWidth = Camera.main.pixelWidth;

		recipeBook = new RecipeBook ();
		recipeBook.loadRecipeBook ();

		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		recipeBookPanel = Instantiate (recipeBookPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
		recipeBookPanel.transform.position = new Vector3 (cameraWidth * 2, cameraHeight / 2, 0f);

		LayoutRecipeBook ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C))
			ToggleRecipeBook ();
	}

	void LayoutRecipeBook()
	{
		Text recipeText = recipeBookPanel.AddComponent<Text> ();
		recipeText.text = recipeBook.toString();
		recipeText.fontSize = 72;
		recipeText.color = Color.magenta;
	}

	void ToggleRecipeBook()
	{
		showRecipeBook = !showRecipeBook;

		if (showRecipeBook)
			recipeBookPanel.transform.position = new Vector3 (cameraWidth / 2, cameraHeight / 2, 0f);
		else
			recipeBookPanel.transform.position = new Vector3 (cameraWidth * 2, cameraHeight / 2, 0f);
	}
}
