using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	public GameObject recipeButtonPrefab;

    public GameObject recipeSlotPrefab;

	public GameObject recipeBookPanel;

    public InventoryController inventoryController;

    public Inventory inventory;

	public RecipeBook recipeBook;

    public Recipe selectedRecipe;

	bool showRecipeBook = false;

	float cameraHeight;
	float cameraWidth;

    protected const string recipesContainerName = "RecipesContainer";

    protected const string ingredientsContainerName = "IngredientsContainer";

    protected const string productImageName = "RecipeProductImage";

    protected const string craftButtonName = "CraftButton";

	// Use this for initialization
	void Start () {
		
		cameraHeight = Camera.main.pixelHeight;
		cameraWidth = Camera.main.pixelWidth;

		recipeBook = new RecipeBook ();
		recipeBook.loadRecipeBook ();
        selectedRecipe = recipeBook.recipes[0];

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

            slot.GetComponent<RecipeSlot>().recipe = recipe;

            //Need to set click event handler to set the current recipe.
            /*
            Image recipeImage = slot.GetComponent<Image>();
            recipeImage.sprite = recipe.productSprite;
            recipeImage.color = Color.white;
            */
            Image slotImage = slot.GetComponent<Image>();
            slotImage.sprite = recipe.productSprite;
            slotImage.color = Color.white;

            //slot.GetComponent<RecipeSlot>().SetOnClick(selectRecipe(recipe));
        }

        displayRecipe();

        GameObject craftButton = recipeBookPanel.transform.Find(craftButtonName).gameObject;

        //Need to set click event handler to craft the current recipe.
	}

    public void selectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;
        displayRecipe();
    }

    public void displayRecipe() {

        Image productImage = recipeBookPanel.transform.Find(productImageName).gameObject.GetComponent<Image>();
        productImage.sprite = selectedRecipe.productSprite;
    }

	public void CraftItem (Recipe recipe) {
		
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
