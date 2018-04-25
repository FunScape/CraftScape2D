using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler {

    public Recipe recipe;
    
    public RecipeSlot() {}

    /*public void SetOnClick(eventHandlerFunction function) {

        onClick = function;
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject player = GameManager.instance.LocalPlayer();
        player.GetComponent<PlayerRecipeBookController>().SelectRecipe(recipe);
    }
}
