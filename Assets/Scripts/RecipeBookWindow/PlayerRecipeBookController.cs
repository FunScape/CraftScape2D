using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	public GameObject recipeButtonPrefab;

    public GameObject recipeSlotPrefab;

	public GameObject recipeBookPanel;

    public InventoryController inventoryController;

    public Inventory inventory;

	public RecipeBook recipeBook;

	bool showRecipeBook = false;

	float cameraHeight;
	float cameraWidth;

    protected const string recipesContainerName = "RecipesContainer";

    protected const string ingredientsContainerName = "IngredientsContainer";

	// Use this for initialization
	void Start () {
		
		cameraHeight = Camera.main.pixelHeight;
		cameraWidth = Camera.main.pixelWidth;

		recipeBook = new RecipeBook ();
		recipeBook.loadRecipeBook ();

		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		recipeBookPanel = Instantiate (recipeBookPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
        recipeBookPanel.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width + 1000f, 0f, 0f);

        inventoryController = GameObject.FindWithTag("Player").GetComponent<InventoryController>();
        inventory = inventoryController.inventory;

        LayoutRecipeBook ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C))
			ToggleRecipeBook ();
	}

	void LayoutRecipeBook() {

        GameObject recipesContainer = recipeBookPanel.transform.Find(recipesContainerName).gameObject;

		foreach (Recipe recipe in recipeBook.recipes) {
            GameObject slot = Instantiate(
                recipeSlotPrefab,
                Vector3.zero,
                Quaternion.identity,
                recipesContainer.transform
            );
            
            Image recipeSprite = slot.GetComponent<Image>();
            //recipeSprite.sprite = product.sprite;

        }
	}

    void displayRecipe(Recipe recipe) {
        //
    }

	void CraftItem (Recipe recipe) {
		
		bool hasRequiredIngredients = true;

		foreach (RecipeRequirement ingredient in recipe.ingredients) {
			hasRequiredIngredients = hasRequiredIngredients && (inventory.checkQuantity (ingredient.ingredientId) >= ingredient.ingredientQuantity);
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
			recipeBookPanel.transform.localPosition = new Vector3 (-cameraWidth / 3, 0f, 0f);
		else
			recipeBookPanel.transform.localPosition = new Vector3 (cameraWidth * 2, 0f, 0f);
	}
}
