using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRequirement
{

    public int ingredientId;
    public int ingredientQuantity;
    public Sprite ingredientSprite;

    public RecipeRequirement(int ingredientId, int ingredientQuantity, Sprite ingredientSprite)
    {
        this.ingredientId = ingredientId;
        this.ingredientQuantity = ingredientQuantity;
        this.ingredientSprite = ingredientSprite;
    }
}