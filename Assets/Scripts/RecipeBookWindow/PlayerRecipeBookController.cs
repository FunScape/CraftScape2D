using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	public GameObject recipeButtonPrefab;

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

	void LayoutRecipeBook() {

		foreach (Recipe recipe in recipeBook.recipes) {
			GameObject recipeButtonObject = (GameObject)Instantiate (recipeButtonPrefab, Vector3.zero, Quaternion.identity, recipeBookPanel.transform);
			Button recipeButton = recipeButtonObject.GetComponent<Button> ();
			recipeButton.GetComponentInChildren<Text>().text = recipeBook.getItemTitle (recipe.productID);
			recipeButton.onClick.AddListener (delegate{CraftItem (recipe);});
		}
	}

	void CraftItem (Recipe recipe) {
		
		Debug.Log ("Finding inventory...");
		PlayerInventoryController inventoryController = GameObject.FindGameObjectWithTag ("Player").transform.GetComponentsInChildren<PlayerInventoryController> () [0] as PlayerInventoryController;
		Inventory inventory = inventoryController.inventory;
		bool hasRequiredIngredients = true;

		foreach (RecipeRequirement ingredient in recipe.ingredients) {
			hasRequiredIngredients = hasRequiredIngredients && inventory.checkQuantity (ingredient.ingredientId, ingredient.ingredientQuantity);
			Debug.Log ("Checking ingredient: " + ingredient.ingredientId.ToString() + " x " + ingredient.ingredientQuantity.ToString() + ": " + hasRequiredIngredients);
		}

		if (hasRequiredIngredients) {
			Debug.Log ("Ingredients found...");
			foreach (RecipeRequirement ingredient in recipe.ingredients) {
				inventory.RemoveQtyOfItems (ingredient.ingredientId, ingredient.ingredientQuantity);
			}

			inventory.AddItem (recipe.productID);

			inventoryController.UpdateInventoryPanelUI ();
		}
	}

	void ToggleRecipeBook() {
		
		showRecipeBook = !showRecipeBook;

		if (showRecipeBook)
			recipeBookPanel.transform.position = new Vector3 (cameraWidth / 2, cameraHeight / 2, 0f);
		else
			recipeBookPanel.transform.position = new Vector3 (cameraWidth * 2, cameraHeight / 2, 0f);
	}
}
