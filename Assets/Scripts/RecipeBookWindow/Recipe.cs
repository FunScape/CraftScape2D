using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Recipe{

	public int id;
	public int productID;
	public int productQuantity;
	public List<RecipeRequirement> ingredients;
	public bool canCraft = false;

	public Recipe(int id, int product, int productQuantity, List<RecipeRequirement> ingredients) {
		this.id = id;
		this.productID = product;
		this.productQuantity = productQuantity;
		this.ingredients = ingredients;
	}

	public Recipe(string filePath) {
		this.ingredients = new List<RecipeRequirement> ();

		JsonData recipeRequirements = JsonMapper.ToObject (File.ReadAllText (filePath));
		foreach (JsonData requirement in recipeRequirements) {
			Debug.Log ("Message:");
			Debug.Log (requirement.ToString ());
			ingredients.Add (new RecipeRequirement((int)requirement ["ingredientId"], (int)requirement ["ingredientQuantity"]));
		}

		this.id = (int)recipeRequirements [0] ["recipeId"];
		this.productID = (int)recipeRequirements [0] ["productId"];
		this.productQuantity = (int)recipeRequirements [0] ["productQuantity"];
	}
}
