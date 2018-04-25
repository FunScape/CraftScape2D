using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Recipe {

	public int id;
    public StaticGameItem product;
	public int quantity;
    public string skillType;
    public List<RecipeRequirement> ingredients;
    public bool canCraft = false;

    public int expCost;
    public int expReward;

	public Recipe(int id, StaticGameItem product, int quantity, string skillType, List<RecipeRequirement> ingredients, int expCost, int expReward, bool canCraft) {
		this.id = id;
		this.product = product;
		this.quantity = quantity;
        this.skillType = skillType;
		this.ingredients = ingredients;
        this.expCost = expCost;
        this.expReward = expReward;
        this.canCraft = canCraft;
	}
    //I think this constructor needs to go, along with the next one.
    /*public Recipe(JsonData recipeJson, JsonData ingredientsJson) {

        database = new Database();

        id = (int)recipeJson["id"];
        productID = (int)recipeJson["productId"];
        productQuantity = (int)recipeJson["productQuantity"];

        InventoryItem productItem = database.GetItem(productID);
        productSprite = productItem.sprite;

        ingredients = new List<RecipeRequirement>();

        foreach (JsonData ingredient in ingredientsJson) {

            InventoryItem ingredientItem = database.GetItem((int)ingredient["ingredientId"]);
            ingredients.Add(new RecipeRequirement((int)ingredient["ingredientId"], (int)ingredient["ingredientQuantity"], ingredientItem.sprite));
        }
    }*/

    //This one will probably end up being obsolete by the time I'm done creating a constructor that takes JsonData arguments.
	/*public Recipe(string recipeJSON, string ingredientsJSON) {

        database = new Database();

		this.ingredients = new List<RecipeRequirement> ();

		JsonData recipe = JsonMapper.ToObject (File.ReadAllText (recipeJSON));
        //Pull down recipe requirements
        JsonData recipeRequirements = JsonMapper.ToObject(File.ReadAllText(ingredientsJSON));
		foreach (JsonData requirement in recipeRequirements) {

            InventoryItem ingredientItem = database.GetItem((int)requirement["ingredientId"]);
            ingredients.Add (new RecipeRequirement((int)requirement ["ingredientId"], (int)requirement ["ingredientQuantity"], ingredientItem.sprite));
			//int ing = (int)requirement ["ingredientId"];
			//int qty = (int)requirement ["ingredientQuantity"];
			//RecipeRequirement recReq = new RecipeRequirement (ing, qty);
			//ingredients.Add (recReq);

		}

		this.id = (int)recipe [0] ["id"];
		this.productID = (int)recipe [0] ["productId"];
		this.productQuantity = (int)recipe [0] ["productQuantity"];

        InventoryItem productItem = database.GetItem(this.productID);
        this.productSprite = productItem.sprite;
	}*/

    public static Recipe Parse(JsonData data, bool canCraft)
    {
        int id = (int)data["id"];
        int quantity = (int)data["quantity"];
        StaticGameItem product = StaticGameItem.Parse(data["static_game_item"]);

        List<RecipeRequirement> ingredients = new List<RecipeRequirement>();

        foreach(JsonData ingredient in data["ingredients"])
        {
            ingredients.Add(RecipeRequirement.Parse(ingredient));
        }

        string skillType = data["skill_type"].ToString();
        float expCost = (float)(double)data["exp_cost"]; //For some reason, JsonData objects cannot be cast directly to floats. They can either be cast to doubles or ints, which can then be cast to floats.
        float expReward = (float)(double)data["exp_reward"];

        Recipe recipe = new Recipe(id, product, quantity, skillType, ingredients, (int)expCost, (int)expReward, canCraft);
        return recipe;
    }
}
