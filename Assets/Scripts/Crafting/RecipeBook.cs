using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class RecipeBook {

    //protected Database itemDatabase;

	public List<Recipe> recipes;

	/*public void loadRecipeBook() {

		itemDatabase = new Database ();

		recipes = new List<Recipe> ();

        JsonData recipesJSON = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + recipePath));

        foreach (JsonData recipe in recipesJSON) {

            JsonData ingredientsJson = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + ingredientsPath + recipe["id"] + ".json"));

            recipes.Add(new Recipe(recipe, ingredientsJson));
        }
    }*/

    public RecipeBook()
    {
        recipes = new List<Recipe>();
        Load();
    }

	//A method for printing a recipe book as a string, just for testing.
	public string toString() {

		string recipeBook = "";

		foreach(Recipe recipe in recipes) {
			foreach(RecipeRequirement requirement in recipe.ingredients) {
				recipeBook += requirement.quantity.ToString() + " " + requirement.ingredient.Name + "(s),";
			}

			recipeBook += " make(s) " + recipe.quantity.ToString () + " " + recipe.product.Name + "(s)." + System.Environment.NewLine;
		}

		return recipeBook;
	}

    public void Load()
    {
        if (PlayerPrefs.GetInt("IsLocalPlayer") == 1)
            return;
        
        APIManager apiManager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();
        apiManager.StartCoroutine(apiManager.GetCharacterSkills((recipes) => {
            this.recipes = recipes;
        }));
    }
}