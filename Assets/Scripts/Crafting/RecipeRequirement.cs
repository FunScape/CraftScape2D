using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class RecipeRequirement {

    public StaticGameItem ingredient;
    public int quantity;

	public RecipeRequirement(StaticGameItem ingredient, int quantity) {
        this.ingredient = ingredient;
        this.quantity = quantity;
	}

    public static RecipeRequirement Parse(JsonData data)
    {
        StaticGameItem ingredient = StaticGameItem.Parse(data["static_game_item"]);
        int quantity = (int)data["quantity"];

        RecipeRequirement recipeRequirement = new RecipeRequirement(ingredient, quantity);
        return recipeRequirement;
    }
}
