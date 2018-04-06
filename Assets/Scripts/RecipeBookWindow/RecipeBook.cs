using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class RecipeBook {

    private string recipePath = "/GameData/testingRecipeJSON/RecipeDatabase.json";

    private string ingredientsPath = "/GameData/testingRecipeJSON/ingredients/";

    protected Database itemDatabase;

	public List<Recipe> recipes;

	public void loadRecipeBook() {

		itemDatabase = new Database ();

		recipes = new List<Recipe> ();

        JsonData recipesJSON = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + recipePath));

        foreach (JsonData recipe in recipesJSON) {

            JsonData ingredientsJson = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + ingredientsPath + recipe["id"] + ".json"));

            recipes.Add(new Recipe(recipe, ingredientsJson));
        }
    }

	//A method for printing a recipe book as a string, just for testing.
	public string toString() {

		string recipeBook = "";

		foreach(Recipe recipe in recipes) {
			foreach(RecipeRequirement requirement in recipe.ingredients) {
				recipeBook += requirement.ingredientQuantity.ToString() + " " + itemDatabase.GetItem (requirement.ingredientId).title + "(s),";
			}

			recipeBook += " make(s) " + recipe.productQuantity.ToString () + " " + itemDatabase.GetItem (recipe.productID).title + "(s)." + System.Environment.NewLine;
		}

		return recipeBook;
	}

	public string getItemTitle(int itemId) {
		return itemDatabase.GetItem (itemId).title;
	}
}