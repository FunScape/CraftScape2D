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
		for (int i = 0; i < recipeRequirements.Count; i++) {
			ingredients.Add (new RecipeRequirement((int)recipeRequirements [i]["ingredientId"], (int)recipeRequirements [i] ["ingredientQuantity"]));
		}

		this.id = (int)recipeRequirements [0] ["recipeId"];
		this.productID = (int)recipeRequirements [0] ["productId"];
		this.productQuantity = (int)recipeRequirements [0] ["productQuantity"];
	}
}
