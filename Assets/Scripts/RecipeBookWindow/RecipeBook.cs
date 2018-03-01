using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook {

	protected Database itemDatabase;

	public List<Recipe> recipes;

	public void loadRecipeBook() {
		recipes = new List<Recipe> ();
		//Read list of recipes unlocked from database.
		//for (each recipe unlocked):
		//read requirements from the database
		Recipe recipe = new Recipe(Application.streamingAssetsPath + "/GameData/testingRecipeJSON/RecipeDatabase.json");
		recipes.Add (recipe);
		//end for
}
	//A method for printing a recipe book as a string, just for testing.
	public string toString() {
		string recipeBook = "";

		foreach(Recipe recipe in recipes) {
			foreach(RecipeRequirement requirement in recipe.ingredients) {
				recipeBook = string.Concat(recipeBook, requirement.ingredientQuantity.ToString(), " ", itemDatabase.GetIem (requirement.ingredientId).title, "(s),");
			}

			recipeBook = string.Concat (recipeBook, " make(s) ", recipe.productQuantity.ToString (), " ", itemDatabase.GetItem (recipe.productID).title, "(s).", System.Environment.NewLine);
		}

		return recipeBook;
	}
}