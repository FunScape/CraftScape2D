using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRequirement {

	public int ingredientId;
	public int ingredientQuantity;

	public RecipeRequirement(int ingredientId, int ingredientQuantity) {
		this.ingredientId = ingredientId;
		this.ingredientQuantity = ingredientQuantity;
	}
}
