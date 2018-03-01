using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	GameObject recipeBookPanel;

	public RecipeBook recipeBook;

	// Use this for initialization
	void Start () {
		recipeBook = new RecipeBook ();
		recipeBook.loadRecipeBook ();

		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		recipeBookPanel = Instantiate (recipeBookPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);

		LayoutRecipeBook ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LayoutRecipeBook()
	{
		Text text = recipeBookPanel.AddComponent<Text> ();
		text.text = recipeBook.toString();
	}
}
